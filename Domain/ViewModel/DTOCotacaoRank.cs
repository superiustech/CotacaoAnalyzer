namespace Domain.ViewModel
{
    public class DTOCotacaoRank
    {
        public int CodigoCotacao { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
        public bool FreteIncluso { get; set; }
        public decimal ValorTotal { get; set; }
        public int Rank { get; set; }
        public decimal Score { get; set; }
        public List<DTOCotacaoItem> Itens { get; set; } = new();
    }
}
