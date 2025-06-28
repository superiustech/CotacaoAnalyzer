using Domain.Entities;
using Domain.Interfaces;
using Domain.ViewModel;
using Domain.Enumeradores;
using IBID.WebService.Domain.Uteis;
using AutoMapper;
using Infra.Repositories;
namespace Bussiness.Services
{
    public class CotacaoService : ICotacaoService
    {
        private readonly ICotacaoRepository _cotacaoRepository;
        private readonly IEntidadeLeituraRepository _entidadeLeituraRepository;
        private readonly IMapper _mapper;
        public CotacaoService(ICotacaoRepository cotacaoRepository, IEntidadeLeituraRepository entidadeLeituraRepository, IMapper mapper)
        {
            _cotacaoRepository = cotacaoRepository;
            _entidadeLeituraRepository = entidadeLeituraRepository;
            _mapper = mapper;
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
        private async Task<bool> ValidarCotacaoExistenteAsync(int codigoCotacao)
        {
            CWCotacao cwCotacao = await _entidadeLeituraRepository.Consultar<CWCotacao>(x => x.nCdCotacao == codigoCotacao);
            return cwCotacao != null;
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
