using Domain.ViewModel;

namespace Domain.Interfaces
{
    public interface ICotacaoService
    {
        Task<List<DTOCotacao>> PesquisarCotacoes();
        Task<DTORetorno> CadastrarCotacao(DTOCotacao oDTOCotacao);
        Task<DTORetorno> EditarCotacao(DTOEditarCotacao oDTOCotacao);
    }
}
