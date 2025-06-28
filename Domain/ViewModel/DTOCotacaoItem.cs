namespace Domain.ViewModel
{
    public class DTOCotacaoItem
    {
        public int CodigoCotacaoItem { get; set; }
        public int Sequencial { get; set; }
        public int PrazoEntrega { get; set; }
        public decimal ValorProposto { get; set; }
        public DTOProduto Produto { get; set; } = new();
    }
}
