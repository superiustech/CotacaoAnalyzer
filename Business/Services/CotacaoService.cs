using Domain.Entities;
using Domain.Interfaces;
using Domain.ViewModel;
using Domain.Enumeradores;
using IBID.WebService.Domain.Uteis;
using AutoMapper;
using Infra.Repositories;
using Domain.ViewModel.Requests;
namespace Bussiness.Services
{
    public class CotacaoService : ICotacaoService
    {
        private readonly ICotacaoRepository _cotacaoRepository;
        private readonly IEntidadeLeituraRepository _entidadeLeituraRepository;
        private readonly IScoreRepository _scoreRepository;
        private readonly IProdutoService _produtoService;
        private readonly IMapper _mapper;
        public CotacaoService(ICotacaoRepository cotacaoRepository, IEntidadeLeituraRepository entidadeLeituraRepository, IMapper mapper, IScoreRepository scoreRepository, IProdutoService produtoService)
        {
            _cotacaoRepository = cotacaoRepository;
            _entidadeLeituraRepository = entidadeLeituraRepository;
            _mapper = mapper;
            _scoreRepository = scoreRepository;
            _produtoService = produtoService;
        }
        public async Task<List<DTOCotacao>> PesquisarCotacoes()
        {
            try 
            { 
                List<CWCotacao> lstCotacoes = await _cotacaoRepository.PesquisarCotacoes();
                return _mapper.Map<List<DTOCotacao>>(lstCotacoes);
            }
            catch
            {
                throw;
            }
        }
        public async Task<DTORetorno> CadastrarCotacao(DTOCotacao oDTOCotacao)
        {
            try
            {
                if(await ValidarCotacaoExistenteAsync(oDTOCotacao.CodigoCotacao))
                {
                    throw new ExcecaoCustomizada($"Cotação com código '{oDTOCotacao.CodigoCotacao}' já criada no sistema.");
                }

                CWCotacao entidadeCotacao = _mapper.Map<CWCotacao>(oDTOCotacao);
                CWCotacao cwCotacao = await _cotacaoRepository.CadastrarCotacao(entidadeCotacao);
                return new DTORetorno() { Status = enumSituacaoRetorno.Sucesso, Mensagem = $"Cotação de código {cwCotacao.nCdCotacao} cadastrado com sucesso no sistema." };
            }
            catch
            {
                throw;
            }
        }
        public async Task<DTORetorno> EditarCotacao(DTOEditarCotacao oDTOCotacao)
        {
            try
            {
                if (oDTOCotacao.CodigoCotacao <= 0) throw new ExcecaoCustomizada($"O código com código '{oDTOCotacao.CodigoCotacao}' da cotação é obrigatório");

                await _cotacaoRepository.EditarCotacao(MapearEdicaoCotacao(oDTOCotacao));

                return new DTORetorno() { Status = enumSituacaoRetorno.Sucesso, Mensagem = "Cotação editada com sucesso no sistema." };
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<DTOCotacaoRank>> CompararCotacoes(DTOCotacaoAnalise dto)
        {
            var peso = await _scoreRepository.ObterScore(dto.CodigoScore);
            var cotacoes = await ObterCotacoesFiltradas(dto.CodigosCotacoes);
            var codigosComunsCotacao = _produtoService.GarantirProdutosComuns(cotacoes);
            var normalizacao = CalcularNormalizacao(cotacoes, codigosComunsCotacao);
            var resultados = cotacoes.Select(c => CalcularResultado(c, peso, codigosComunsCotacao, normalizacao)).ToList();
            AtribuirRanking(resultados);
            return resultados;
        }
        public async Task<DTORetorno> AdicionarItens(DTOAdicionarItensRequest dto)
        {
            if (!await ValidarCotacaoExistenteAsync(dto.CodigoCotacao))
                throw new ExcecaoCustomizada($"Cotação com código '{dto.CodigoCotacao}' não existente no sistema.");

            var cotacao = await _entidadeLeituraRepository.Consultar<CWCotacao>(x => x.nCdCotacao == dto.CodigoCotacao);
            var itensExistentes = await _entidadeLeituraRepository.Pesquisar<CWCotacaoItem>(x => x.nCdCotacao == dto.CodigoCotacao);
            var proximoSequencial = (itensExistentes.Any() ? itensExistentes.Max(x => x.nSequencial) : 0) + 1;
            var novosItens = _mapper.Map<List<CWCotacaoItem>>(dto.Itens);

            foreach (var item in novosItens)
            {
                item.nCdCotacao = dto.CodigoCotacao;
                item.nSequencial = proximoSequencial++;
            }

            await _cotacaoRepository.AdicionarItens(cotacao, novosItens);

            return new DTORetorno { Status = enumSituacaoRetorno.Sucesso, Mensagem = "Itens adicionados à cotação com sucesso."};
        }

        private void AtribuirRanking(List<DTOCotacaoRank> resultados)
        {
            var ordenados = resultados.OrderByDescending(r => r.Score).ToList();
            for (int i = 0; i < ordenados.Count; i++)
                ordenados[i].Rank = i + 1;
        }
        public async Task<bool> ValidarCotacaoExistenteAsync(int codigoCotacao)
        {
            CWCotacao cwCotacao = await _entidadeLeituraRepository.Consultar<CWCotacao>(x => x.nCdCotacao == codigoCotacao);
            return cwCotacao != null;
        }
        private async Task<List<CWCotacao>> ObterCotacoesFiltradas(List<int> codigos)
        {
            var todas = await _cotacaoRepository.PesquisarCotacoes();
            return todas.Where(c => codigos.Contains(c.nCdCotacao)).ToList();
        }
        private (decimal maxPreco, decimal minPreco, int maxPrazo, int minPrazo) CalcularNormalizacao(List<CWCotacao> cotacoes, List<int> codigosComunsCotacao)
        {
            var all = cotacoes.SelectMany(c => c.lstCotacaoItem).Where(i => codigosComunsCotacao.Contains(i.nCdProduto)).ToList();
            return (all.Max(i => i.dVlProposto), all.Min(i => i.dVlProposto), all.Max(i => i.nPrazoEntrega), all.Min(i => i.nPrazoEntrega));
        }
        private DTOCotacaoRank CalcularResultado(CWCotacao cot, CWScore peso, List<int> codigosComunsCotacao, (decimal maxPreco, decimal minPreco, int maxPrazo, int minPrazo) norm)
        {
            decimal score = 0m;

            foreach (var item in cot.lstCotacaoItem.Where(i => codigosComunsCotacao.Contains(i.nCdProduto)))
            {
                var precoNorm = norm.maxPreco != norm.minPreco ? (norm.maxPreco - item.dVlProposto) / (norm.maxPreco - norm.minPreco) : 1m;
                var prazoNorm = norm.maxPrazo != norm.minPrazo ? (norm.maxPrazo - item.nPrazoEntrega) / (norm.maxPrazo - norm.minPrazo) : 1m;
                var freteScore = cot.bFlFreteIncluso ? 1m : 0m;
                score += peso.nPesoValor * precoNorm + peso.nPesoFreteIncluso * freteScore + peso.nPesoPrazoEntrega * prazoNorm;
            }

            return new DTOCotacaoRank
            {
                CodigoCotacao = cot.nCdCotacao,
                Descricao = cot.sDsCotacao,
                Data = cot.tDtCotacao,
                FreteIncluso = cot.bFlFreteIncluso,
                Itens = _mapper.Map<List<DTOCotacaoItem>>(cot.lstCotacaoItem),
                ValorTotal = cot.lstCotacaoItem.Where(x => x.dVlProposto > 0).Sum(x => x.dVlProposto),
                Score = score
            };
        }
        private CWCotacao MapearEdicaoCotacao(DTOEditarCotacao oDTOCotacao)
        {
            return new CWCotacao()
            {
                nCdCotacao = oDTOCotacao.CodigoCotacao,
                sDsCotacao = oDTOCotacao.Descricao,
                dVlTotal = oDTOCotacao.ValorTotal,
                tDtCotacao = oDTOCotacao.Data,
                bFlFreteIncluso = oDTOCotacao.FreteIncluso
            };
        }
    }
    
}
