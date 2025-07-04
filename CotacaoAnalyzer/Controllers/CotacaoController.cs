using CotacaoAnalyzer.Uteis;
using Domain.Enumeradores;
using Domain.Interfaces;
using Domain.ViewModel;
using Domain.ViewModel.Requests;
using IBID.WebService.Domain.Uteis;
using Microsoft.AspNetCore.Mvc;
using System.Net;
namespace CotacaoAnalyzer.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CotacaoController : ControllerBase
    {
        private readonly ICotacaoService _cotacao;

        public CotacaoController(ICotacaoService cotacao)
        {
            _cotacao = cotacao;
        }
        /// <summary>
        /// Retorna todos as cotações cadastrados.
        /// </summary>
        [HttpGet("Cotacoes")]
        public async Task<IActionResult> Cotacoes()
        {
            try
            {
                return Ok(await _cotacao.PesquisarCotacoes());
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
        /// Retorna o ranking com as melhores cotações.
        /// </summary>
        [HttpPost("CompararCotacoes")]
        public async Task<IActionResult> CompararCotacoes(DTOCotacaoAnalise dtoCotacao)
        {
            try
            {
                return Ok(await _cotacao.CompararCotacoes(dtoCotacao));
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
        /// Cadastra um cotação criada no sistema e cria novos produtos caso não exista.
        /// </summary>
        [HttpPost("CadastrarCotacao")]
        public async Task<IActionResult> CadastrarCotacao(DTOCotacao dtoCotacao)
        {
            try
            {
                return Ok(await _cotacao.CadastrarCotacao(dtoCotacao));
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
        /// Cadastra um cotação criada no sistema e cria novos produtos caso não exista.
        /// </summary>
        [HttpPost("AdicionarItens")]
        public async Task<IActionResult> AdicionarItens(DTOAdicionarItensRequest dtoAdicionarItem)
        {
            try
            {
                return Ok(await _cotacao.AdicionarItens(dtoAdicionarItem));
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
        /// Edita um cotação existente no sistema.
        /// </summary>
        [HttpPut("EditarCotacao")]
        public async Task<IActionResult> EditarCotacao(DTOEditarCotacao dtoCotacao)
        {
            try
            {
                return Ok(await _cotacao.EditarCotacao(dtoCotacao));
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
