using CotacaoAnalyzer.Uteis;
using Domain.Enumeradores;
using Domain.Interfaces;
using Domain.ViewModel;
using IBID.WebService.Domain.Uteis;
using Microsoft.AspNetCore.Mvc;
using System.Net;
namespace CotacaoAnalyzer.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoService _produto;

        public ProdutoController(IProdutoService produto)
        {
            _produto = produto;
        }
        /// <summary>
        /// Retorna todos os produtos cadastrados.
        /// </summary>
        [HttpGet("Produtos")]
        public async Task<IActionResult> Produtos()
        {
            try
            {
                return Ok(await _produto.PesquisarProdutos());
            }
            catch (ExcecaoCustomizada ex)
            {
                return UtilitarioResposta.CriarResposta(this, HttpStatusCode.NotFound, enumSituacaoRetorno.Erro, ex.Message, true);
            }
            catch (Exception ex)
            {
                return UtilitarioResposta.CriarResposta(this, HttpStatusCode.BadRequest, enumSituacaoRetorno.Erro, ex.Message);
            }
        }
        /// <summary>
        /// Cadastra um produto no sistema.
        /// </summary>
        [HttpPost("CadastrarProduto")]
        public async Task<IActionResult> CadastrarProduto(DTOProduto dtoProduto)
        {
            try
            {
                return Ok(await _produto.CadastrarProduto(dtoProduto));
            }
            catch (ExcecaoCustomizada ex)
            {
                return UtilitarioResposta.CriarResposta(this, HttpStatusCode.NotFound, enumSituacaoRetorno.Erro, ex.Message, true);
            }
            catch (Exception ex)
            {
                return UtilitarioResposta.CriarResposta(this, HttpStatusCode.BadRequest, enumSituacaoRetorno.Erro, ex.Message);
            }
        }
        /// <summary>
        /// Edita um produto existente no sistema.
        /// </summary>
        [HttpPut("EditarProduto")]
        public async Task<IActionResult> EditarProduto(DTOProduto dtoProduto)
        {
            try
            {
                return Ok(await _produto.EditarProduto(dtoProduto));
            }
            catch (ExcecaoCustomizada ex)
            {
                return UtilitarioResposta.CriarResposta(this, HttpStatusCode.NotFound, enumSituacaoRetorno.Erro, ex.Message, true);
            }
            catch (Exception ex)
            {
                return UtilitarioResposta.CriarResposta(this, HttpStatusCode.BadRequest, enumSituacaoRetorno.Erro, ex.Message);
            }
        }
    }
}
