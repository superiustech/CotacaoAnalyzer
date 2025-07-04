
using Domain.Enumeradores;

namespace Domain.ViewModel
{
    public class DTORetorno
    {
        public enumSituacaoRetorno Status { get; set; }
        public object Id { get; set; }
        public string Mensagem { get; set; }
    }
}
