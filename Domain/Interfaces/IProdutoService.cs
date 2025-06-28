using Domain.ViewModel;

namespace Domain.Interfaces
{
    public interface IProdutoService
    {
        Task<List<DTOProduto>> PesquisarProdutos();
        Task<DTORetorno> CadastrarProduto(DTOProduto oDTOProduto);
        Task<DTORetorno> EditarProduto(DTOProduto oDTOProduto);
    }
}
