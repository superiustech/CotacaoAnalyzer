using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IScoreRepository
    {
        Task<List<CWScore>> PesquisarScores();
        Task<CWScore> CadastrarScore(CWScore cwScore);
        Task<CWScore> EditarScore(CWScore cwScore);
        Task<CWScore> ObterScore(int codigoScore);
    }
}
