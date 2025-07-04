namespace Domain.ViewModel.Requests
{
    public class DTOAdicionarItensRequest
    {
        public int CodigoCotacao { get; set; }
        public List<DTOCotacaoItem> Itens { get; set; } = new();
    }
}
