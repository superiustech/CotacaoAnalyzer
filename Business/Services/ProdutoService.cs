using Domain.Entities;
using Domain.Interfaces;
using Domain.ViewModel;
using Domain.Enumeradores;
using IBID.WebService.Domain.Uteis;
using AutoMapper;
namespace Bussiness.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IEntidadeLeituraRepository _entidadeLeituraRepository;
        private readonly IMapper _mapper;
        public ProdutoService(IProdutoRepository produtoRepository, IEntidadeLeituraRepository entidadeLeituraRepository, IMapper mapper)
        {
            _produtoRepository = produtoRepository;
            _entidadeLeituraRepository = entidadeLeituraRepository;
            _mapper = mapper;
        }
        public async Task<List<DTOProduto>> PesquisarProdutos()
        {
            try 
            { 
                List<CWProduto> lstProdutos = await _produtoRepository.PesquisarProdutos();
                return _mapper.Map<List<DTOProduto>>(lstProdutos);
            }
            catch
            {
                throw;
            }
        }
        public async Task<DTORetorno> CadastrarProduto(DTOProduto oDTOProduto)
        {
            try
            {
                if(await ValidarProdutoExistenteAsync(oDTOProduto.CodigoProduto))
                {
                    throw new ExcecaoCustomizada($"Produto com código {oDTOProduto.CodigoProduto} já cadastrado no sistema.");
                }

                CWProduto entidadeProduto = _mapper.Map<CWProduto>(oDTOProduto);
                CWProduto cwProduto = await _produtoRepository.CadastrarProduto(entidadeProduto);
                return new DTORetorno() { Status = enumSituacaoRetorno.Sucesso, Mensagem = $"Produto de código {cwProduto.nCdProduto} cadastrado com sucesso no sistema." };
            }
            catch
            {
                throw;
            }
        }
        public async Task<DTORetorno> EditarProduto(DTOProduto oDTOProduto)
        {
            try
            {
                if (oDTOProduto.CodigoProduto <= 0) throw new ExcecaoCustomizada("O código do produto é obrigatório");
                CWProduto entidadeProduto = _mapper.Map<CWProduto>(oDTOProduto);
                await _produtoRepository.EditarProduto(entidadeProduto);
                return new DTORetorno() { Status = enumSituacaoRetorno.Sucesso, Mensagem = "Produto editado com sucesso no sistema." };
            }
            catch
            {
                throw;
            }
        }
        private async Task<bool> ValidarProdutoExistenteAsync(int codigoProduto)
        {
            CWProduto cwProduto = await _entidadeLeituraRepository.Consultar<CWProduto>(x => x.nCdProduto == codigoProduto);
            return cwProduto != null;
        }
    }
    
}
