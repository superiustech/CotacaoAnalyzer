namespace Domain.ViewModel.Requests
{
    public class DTOEditarCotacao
    {
        public int CodigoCotacao { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
        public bool FreteIncluso { get; set; }
        public decimal ValorTotal { get; set; }
    }
}
