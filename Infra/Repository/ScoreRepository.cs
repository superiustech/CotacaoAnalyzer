using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Infra.Contexts;
using IBID.WebService.Domain.Uteis;

namespace Infra.Repositories
{
    public class ScoreRepository : IScoreRepository
    {
        private readonly AnalyzerDbContext _context;
        public ScoreRepository(AnalyzerDbContext context)
        {
            _context = context;
        }

        public async Task<List<CWScore>> PesquisarScores()
        {
            return await _context.Scores.AsNoTracking().ToListAsync();
        }
        public async Task<CWScore> CadastrarScore(CWScore Score)
        {
            _context.Scores.Add(Score);
            await _context.SaveChangesAsync();
            return Score;
        }
        public async Task<CWScore> EditarScore(CWScore score)
        {
            var existente = await _context.Scores.FindAsync(score.nCdScore);
            if (existente == null) return await CadastrarScore(score);

            existente.nPesoValor = score.nPesoValor;
            existente.nPesoFreteIncluso = score.nPesoFreteIncluso;
            existente.nPesoPrazoEntrega = score.nPesoPrazoEntrega;
            existente.tDtCriacaoPeso = score.tDtCriacaoPeso;

            await _context.SaveChangesAsync();
            return existente;
        }
        public async Task<CWScore> ObterScore(int codigoScore)
        {
            var score = await _context.Scores.AsNoTracking().FirstOrDefaultAsync(x => x.nCdScore == codigoScore) ??
            new CWScore()
            {
                nCdScore = 0,
                nPesoValor = 1,
                nPesoPrazoEntrega = 2,
                nPesoFreteIncluso = 3
            };
            return score;
        }
    }
}
