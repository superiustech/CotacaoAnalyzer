using Domain.ViewModel;
using Domain.ViewModel.Requests;

namespace Domain.Interfaces
{
    public interface ICotacaoService
    {
        Task<List<DTOCotacao>> PesquisarCotacoes();
        Task<DTORetorno> CadastrarCotacao(DTOCotacao oDTOCotacao);
        Task<DTORetorno> EditarCotacao(DTOEditarCotacao oDTOCotacao);
        Task<List<DTOCotacaoRank>> CompararCotacoes (DTOCotacaoAnalise oDTOCotacao);
        Task<bool> ValidarCotacaoExistenteAsync(int codigoCotacao);
        Task<DTORetorno> AdicionarItens(DTOAdicionarItensRequest dtoAdicionarItem);
    }
}
