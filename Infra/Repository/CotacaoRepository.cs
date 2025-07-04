using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Infra.Contexts;
using Domain.Entities;

namespace Infra.Repositories
{
    public class CotacaoRepository : ICotacaoRepository
    {
        private readonly AnalyzerDbContext _context;
        private readonly IProdutoRepository _produtoRepository;
        public CotacaoRepository(AnalyzerDbContext context, IProdutoRepository produtoRepository) 
        { 
            _context = context;
            _produtoRepository = produtoRepository;
        }
        public async Task<List<CWCotacao>> PesquisarCotacoes()
        {
            return await _context.Cotacoes
                    .AsNoTracking()
                    .Include(c => c.lstCotacaoItem)
                    .ThenInclude(p => p.Produto)
                    .ToListAsync();
        }
        public async Task<CWCotacao> CadastrarCotacao(CWCotacao cotacao)
        {
            await _produtoRepository.GarantirProdutos(cotacao.lstCotacaoItem);
            await _context.Cotacoes.AddAsync(cotacao);
            await _context.SaveChangesAsync();
            return cotacao;
        }
        public async Task<CWCotacao> EditarCotacao(CWCotacao cotacao)
        {
            var existente = await _context.Cotacoes.FindAsync(cotacao.nCdCotacao);
            if (existente == null) return await CadastrarCotacao(cotacao);

            existente.sDsCotacao = cotacao.sDsCotacao;
            existente.tDtCotacao = cotacao.tDtCotacao;
            existente.tDtCotacao = cotacao.tDtCotacao;
            existente.bFlFreteIncluso = cotacao.bFlFreteIncluso;

            await _context.SaveChangesAsync();
            return existente;
        }
        public async Task AdicionarItens(CWCotacao cotacao, List<CWCotacaoItem> lstCotacaoItem)
        {
            await _produtoRepository.GarantirProdutos(lstCotacaoItem);
            await _context.CotacaoItens.AddRangeAsync(lstCotacaoItem);
            await _context.SaveChangesAsync();
        }
    }
}
