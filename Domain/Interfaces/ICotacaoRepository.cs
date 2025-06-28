using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ICotacaoRepository
    {
        Task<List<CWCotacao>> PesquisarCotacoes();
        Task<CWCotacao> CadastrarCotacao(CWCotacao cwCotacao);
        Task<CWCotacao> EditarCotacao(CWCotacao cwCotacao);
    }
}
