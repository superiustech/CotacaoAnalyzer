using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Infra.Contexts;

namespace Infra.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly AnalyzerDbContext _context;
        public ProdutoRepository(AnalyzerDbContext context)
        {
            _context = context;
        }

        public async Task<List<CWProduto>> PesquisarProdutos()
        {
            return await _context.Produtos.AsNoTracking().ToListAsync();
        }
        public async Task<CWProduto> CadastrarProduto(CWProduto produto)
        {
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();
            return produto;
        }
        public async Task<CWProduto> EditarProduto(CWProduto produto)
        {
            var existente = await _context.Produtos.FindAsync(produto.nCdProduto);
            if (existente == null) return await CadastrarProduto(produto);

            existente.sNmProduto = produto.sNmProduto;
            existente.sCdProduto = produto.sCdProduto;
            existente.dVlUnitario = produto.dVlUnitario;

            await _context.SaveChangesAsync();
            return existente;
        }
        public async Task GarantirProdutos(List<CWCotacaoItem> itens)
        {
            foreach (var item in itens)
            {
                if (item.Produto != null)
                {
                    var produtoExistente = await _context.Produtos.FindAsync(item.Produto.nCdProduto);

                    if (produtoExistente != null)
                    {
                        item.Produto = produtoExistente;
                    }
                    else
                    {
                        CWProduto novoProduto = await CadastrarProduto(item.Produto);
                        item.Produto = novoProduto;
                    }
                }
            }
        }
    }
}
