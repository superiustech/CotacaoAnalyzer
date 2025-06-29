using Domain.Entities;
using Domain.Interfaces;
using Domain.ViewModel;
using Domain.Enumeradores;
using IBID.WebService.Domain.Uteis;
using AutoMapper;
namespace Bussiness.Services
{
    public class ScoreService : IScoreService
    {
        private readonly IScoreRepository _ScoreRepository;
        private readonly IEntidadeLeituraRepository _entidadeLeituraRepository;
        private readonly IMapper _mapper;
        public ScoreService(IScoreRepository ScoreRepository, IEntidadeLeituraRepository entidadeLeituraRepository, IMapper mapper)
        {
            _ScoreRepository = ScoreRepository;
            _entidadeLeituraRepository = entidadeLeituraRepository;
            _mapper = mapper;
        }
        public async Task<List<DTOScore>> PesquisarScores()
        {
            try 
            { 
                List<CWScore> lstScores = await _ScoreRepository.PesquisarScores();
                return _mapper.Map<List<DTOScore>>(lstScores);
            }
            catch
            {
                throw;
            }
        }
        public async Task<DTORetorno> CadastrarScore(DTOScore oDTOScore)
        {
            try
            {
                if(await ValidarScoreExistenteAsync(oDTOScore.CodigoScore))
                {
                    throw new ExcecaoCustomizada($"Score com código {oDTOScore.CodigoScore} já cadastrado no sistema.");
                }

                CWScore entidadeScore = _mapper.Map<CWScore>(oDTOScore);
                CWScore cwScore = await _ScoreRepository.CadastrarScore(entidadeScore);
                return new DTORetorno() { Status = enumSituacaoRetorno.Sucesso, Mensagem = $"Score de código {cwScore.nCdScore} cadastrado com sucesso no sistema." };
            }
            catch
            {
                throw;
            }
        }
        public async Task<DTORetorno> EditarScore(DTOScore oDTOScore)
        {
            try
            {
                if (oDTOScore.CodigoScore <= 0) throw new ExcecaoCustomizada("O código do Score é obrigatório");
                CWScore entidadeScore = _mapper.Map<CWScore>(oDTOScore);
                await _ScoreRepository.EditarScore(entidadeScore);
                return new DTORetorno() { Status = enumSituacaoRetorno.Sucesso, Mensagem = "Score editado com sucesso no sistema." };
            }
            catch
            {
                throw;
            }
        }
        private async Task<bool> ValidarScoreExistenteAsync(int codigoScore)
        {
            CWScore cwScore = await _entidadeLeituraRepository.Consultar<CWScore>(x => x.nCdScore == codigoScore);
            return cwScore != null;
        }
    }
    
}
