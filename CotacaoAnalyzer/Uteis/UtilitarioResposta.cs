using Domain.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Domain.Enumeradores;
using System.Net;

namespace CotacaoAnalyzer.Uteis
{
    public class UtilitarioResposta
    {
        public static IActionResult CriarResposta (ControllerBase oController, HttpStatusCode enumStatusCode, enumSituacaoRetorno enumSituacaoRetorno,  string strMensagem, bool bFlExceptionCustom = false)
        {
            #if DEBUG
            return oController.StatusCode((int)enumStatusCode, new DTORetorno { Status = enumSituacaoRetorno, Mensagem = strMensagem });
            #endif
            return oController.StatusCode((int)enumStatusCode, new DTORetorno { Status = enumSituacaoRetorno, Mensagem = bFlExceptionCustom ? strMensagem : "Houve um erro não previsto ao processar sua solicitação" });
        }
    }
}
