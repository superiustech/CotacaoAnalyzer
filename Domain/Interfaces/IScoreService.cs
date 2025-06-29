using Domain.ViewModel;

namespace Domain.Interfaces
{
    public interface IScoreService
    {
        Task<List<DTOScore>> PesquisarScores();
        Task<DTORetorno> CadastrarScore(DTOScore oDTOScore);
        Task<DTORetorno> EditarScore(DTOScore oDTOScore);
    }
}
