using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Domain;
using Domain.Interfaces;
using Bussiness.Services;
using Domain.ViewModel;
using Domain.Enumeradores;
using IBID.WebService.Domain.Uteis;
using System.Linq.Expressions;

namespace CotacaoAnalyzerTest.Services
{
    public class ProdutoServiceUnit
    {
        private readonly Mock<IProdutoRepository> _produtoRepositoryMock;
        private readonly Mock<IProdutoService> _produtoServiceMock;
        private readonly Mock<IEntidadeLeituraRepository> _entidadeLeituraRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ProdutoService _produtoService;

        private readonly List<CWProduto> _produtos = new()
        {
            new() { nCdProduto = 1, sNmProduto = "Produto 1" },
            new() { nCdProduto = 2, sNmProduto = "Produto 2" }
        };

        private readonly List<CWCotacao> _cotacoes = new()
        {
            new()
            {
                nCdCotacao = 1,
                lstCotacaoItem = new List<CWCotacaoItem>
                {
                    new()
                    {
                        nCdCotacaoItem = 1,
                        Produto = new CWProduto { nCdProduto = 1, sNmProduto = "Produto 1" }
                    }
                }
            }
        };

        private readonly List<DTOProduto> _produtosDTO = new()
        {
            new() { CodigoProduto = 1, NomeProduto = "Produto 1" },
            new() { CodigoProduto = 2, NomeProduto = "Produto 2" }
        };

        private readonly DTORetorno _dtoSucesso = new()
        {
            Status = enumSituacaoRetorno.Sucesso,
            Mensagem = "Sucesso"
        };

        public ProdutoServiceUnit()
        {
            _produtoRepositoryMock = new Mock<IProdutoRepository>();
            _produtoServiceMock = new Mock<IProdutoService>();
            _entidadeLeituraRepositoryMock = new Mock<IEntidadeLeituraRepository>();
            _mapperMock = new Mock<IMapper>();

            _produtoService = new ProdutoService(
                _produtoRepositoryMock.Object,
                _entidadeLeituraRepositoryMock.Object,
                _mapperMock.Object
            );
        }

        #region Testes Unitários

        [Fact]
        public async Task PesquisarProdutos_RetornarListaDTO()
        {
            _produtoRepositoryMock.Setup(r => r.PesquisarProdutos()).ReturnsAsync(_produtos);
            _mapperMock.Setup(m => m.Map<List<DTOProduto>>(_produtos)).Returns(_produtosDTO);

            var resultado = await _produtoService.PesquisarProdutos();

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);
            Assert.Equal("Produto 1", resultado[0].NomeProduto);

            _produtoRepositoryMock.Verify(r => r.PesquisarProdutos(), Times.Once);
            _mapperMock.Verify(m => m.Map<List<DTOProduto>>(_produtos), Times.Once);
        }

        [Fact]
        public async Task CadastrarProduto_DeveRetornarSucesso_QuandoProdutoNaoExiste()
        {
            var dtoProduto = _produtosDTO.First();
            var entidadeProduto = _produtos.First();

            var retornoEsperado = new DTORetorno
            {
                Status = enumSituacaoRetorno.Sucesso,
                Mensagem = $"Produto de código {entidadeProduto.nCdProduto} cadastrado com sucesso no sistema."
            };

            _produtoServiceMock.Setup(x => x.ValidarProdutoExistenteAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            _produtoRepositoryMock
                .Setup(r => r.CadastrarProduto(It.IsAny<CWProduto>()))
                .ReturnsAsync(entidadeProduto);

            _mapperMock
                .Setup(m => m.Map<CWProduto>(dtoProduto))
                .Returns(entidadeProduto);

            var resultado = await _produtoService.CadastrarProduto(dtoProduto);

            Assert.NotNull(resultado);
            Assert.Equal(enumSituacaoRetorno.Sucesso, resultado.Status);
            Assert.Contains("cadastrado com sucesso", resultado.Mensagem);

            _produtoRepositoryMock.Verify(r => r.CadastrarProduto(It.IsAny<CWProduto>()), Times.Once);
        }

        [Fact]
        public async Task CadastrarProduto_DeveRetornarExcecao_QuandoProdutoExiste()
        {
            var dtoProduto = _produtosDTO.First();
            var entidadeProduto = _produtos.First();

            _produtoServiceMock
                .Setup(x => x.ValidarProdutoExistenteAsync(It.IsAny<int>()))
                .ReturnsAsync(false);

            _entidadeLeituraRepositoryMock
                .Setup(x => x.Consultar<CWProduto>(It.IsAny<Expression<Func<CWProduto, bool>>>()))
                .ReturnsAsync(new CWProduto { nCdProduto = 1 });

            _produtoRepositoryMock
                .Setup(r => r.CadastrarProduto(It.IsAny<CWProduto>()))
                .ReturnsAsync(entidadeProduto);

            _mapperMock
                .Setup(m => m.Map<CWProduto>(dtoProduto))
                .Returns(entidadeProduto);

            var excecao = await Assert.ThrowsAsync<ExcecaoCustomizada>(
                () => _produtoService.CadastrarProduto(dtoProduto));

            Assert.NotNull(excecao);
            Assert.Contains($"Produto com código {dtoProduto.CodigoProduto}", excecao.Message);
        }

        [Fact]
        public async Task EditarProduto_DeveRetornarSucesso_QuandoProdutoExiste()
        {
            var dtoProduto = _produtosDTO.First();
            var entidadeProduto = _produtos.First();

            _mapperMock
                .Setup(m => m.Map<CWProduto>(dtoProduto))
                .Returns(entidadeProduto);

            _produtoRepositoryMock
                .Setup(r => r.EditarProduto(It.IsAny<CWProduto>()))
                .ReturnsAsync(entidadeProduto);

            var resultado = await _produtoService.EditarProduto(dtoProduto);

            Assert.NotNull(resultado);
            Assert.True(resultado.Status == enumSituacaoRetorno.Sucesso);
            Assert.Contains("editado", resultado.Mensagem);
        }

        [Fact]
        public async Task EditarProduto_DeveRetornarExcecao_QuandoCodigoNaoExiste()
        {

            var dtoProduto = new DTOProduto() { CodigoProduto = 0 };
            var entidadeProduto = _produtos.First();

            _mapperMock
                .Setup(m => m.Map<CWProduto>(dtoProduto))
                .Returns(entidadeProduto);

            _produtoRepositoryMock
                .Setup(r => r.EditarProduto(It.IsAny<CWProduto>()))
                .ReturnsAsync(entidadeProduto);

            var excecao = await Assert.ThrowsAsync<ExcecaoCustomizada>(() => _produtoService.EditarProduto(dtoProduto));

            Assert.NotNull(excecao);
            Assert.Contains("é obrigatório", excecao.Message);
        }

        [Fact]
        public void GarantirProdutosComuns_DeveSempreUmaListaInteira()
        {
            var resultado = _produtoService.GarantirProdutosComuns(_cotacoes);
            Assert.NotNull(resultado);
            Assert.IsType<List<int>>(resultado);
        }

        #endregion
    }
}