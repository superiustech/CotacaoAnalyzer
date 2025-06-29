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
    public class ScoreController : ControllerBase
    {
        private readonly IScoreService _score;

        public ScoreController(IScoreService score)
        {
            _score = score;
        }
        /// <summary>
        /// Retorna todos os scores cadastrados.
        /// </summary>
        [HttpGet("Scores")]
        public async Task<IActionResult> Scores()
        {
            try
            {
                return Ok(await _score.PesquisarScores());
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
        /// Cadastra um score no sistema.
        /// </summary>
        [HttpPost("CadastrarScore")]
        public async Task<IActionResult> CadastrarScore(DTOScore dtoScore)
        {
            try
            {
                return Ok(await _score.CadastrarScore(dtoScore));
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
        /// Edita um score existente no sistema.
        /// </summary>
        [HttpPut("EditarScore")]
        public async Task<IActionResult> EditarScore(DTOScore dtoScore)
        {
            try
            {
                return Ok(await _score.EditarScore(dtoScore));
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
