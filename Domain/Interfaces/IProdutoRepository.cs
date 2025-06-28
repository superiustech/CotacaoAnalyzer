using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IProdutoRepository
    {
        Task<List<CWProduto>> PesquisarProdutos();
        Task<CWProduto> CadastrarProduto(CWProduto cwProduto);
        Task<CWProduto> EditarProduto(CWProduto cwProduto);
        Task GarantirProdutos(List<CWCotacaoItem> itens);
    }
}
