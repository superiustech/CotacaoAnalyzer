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
using Domain.ViewModel.Requests;
using Infra.Repositories;

namespace CotacaoAnalyzerTest.Services
{
    public class CotacaoServiceUnit
    {
        private readonly Mock<ICotacaoRepository> _cotacaoRepositoryMock;
        private readonly Mock<IScoreRepository> _scoreRepositoryMock;
        private readonly Mock<ICotacaoService> _cotacaoServiceMock;
        private readonly Mock<IProdutoService> _produtoServiceMock;
        private readonly Mock<IEntidadeLeituraRepository> _entidadeLeituraRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CotacaoService _cotacaoService;

        private readonly List<CWCotacao> _cotacoes = new()
        {
            new() { nCdCotacao = 2, sDsCotacao = "Cotacao 2" , lstCotacaoItem = new List<CWCotacaoItem> { new() { nCdCotacaoItem = 1, nCdProduto = 1}}},
            new() { nCdCotacao = 1, sDsCotacao = "Cotacao 1" , lstCotacaoItem = new List<CWCotacaoItem> { new() { nCdCotacaoItem = 1, nCdProduto = 1}}}
        };
        private readonly List<DTOCotacao> _cotacoesDTO = new()
        {
            new() { CodigoCotacao = 1, Descricao = "Cotacao 1",
            Itens = new List<DTOCotacaoItem>()
            {
                new()
                {
                    CodigoCotacaoItem = 1,
                    PrazoEntrega = 1,
                    Sequencial = 1,
                    ValorProposto = 12
                }
            }},
            new() { CodigoCotacao = 2, Descricao = "Cotacao 2",
            Itens = new List<DTOCotacaoItem>()
            {
                new()
                {
                    CodigoCotacaoItem = 1,
                    PrazoEntrega = 1,
                    Sequencial = 2,
                    ValorProposto = 12
                }
            }}
        };

        DTOEditarCotacao oDTOEditarCotacao = new DTOEditarCotacao(){  CodigoCotacao = 1, Descricao = "Cotacao 1" };
        DTOCotacaoAnalise oDTOCotacaoAnalise = new DTOCotacaoAnalise() { CodigoScore = 1, CodigosCotacoes = new List<int> { 1 }};
        CWScore cwScore = new CWScore() { nCdScore = 1, nPesoFreteIncluso = 1, nPesoPrazoEntrega = 2, nPesoValor = 3 };

        private readonly DTORetorno _dtoSucesso = new()
        {
            Status = enumSituacaoRetorno.Sucesso,
            Mensagem = "Sucesso"
        };

        public CotacaoServiceUnit()
        {
            _cotacaoRepositoryMock = new Mock<ICotacaoRepository>();
            _cotacaoServiceMock = new Mock<ICotacaoService>();
            _entidadeLeituraRepositoryMock = new Mock<IEntidadeLeituraRepository>();
            _produtoServiceMock = new Mock<IProdutoService>();
            _scoreRepositoryMock = new Mock<IScoreRepository>();
            _mapperMock = new Mock<IMapper>();

            _cotacaoService = new CotacaoService(
                _cotacaoRepositoryMock.Object,
                _entidadeLeituraRepositoryMock.Object,
                _mapperMock.Object,
                _scoreRepositoryMock.Object,
                _produtoServiceMock.Object

            );
        }

        #region Testes Unitários

        [Fact]
        public async Task PesquisarCotacoes_RetornarListaDTO()
        {
            _cotacaoRepositoryMock.Setup(r => r.PesquisarCotacoes()).ReturnsAsync(_cotacoes);
            _mapperMock.Setup(m => m.Map<List<DTOCotacao>>(_cotacoes)).Returns(_cotacoesDTO);

            var resultado = await _cotacaoService.PesquisarCotacoes();

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);
        }

        [Fact]
        public async Task CadastrarCotacao_DeveRetornarSucesso_QuandoCotacaoNaoExiste()
        {
            var dtoCotacao = _cotacoesDTO.First();
            var entidadeCotacao = _cotacoes.First();

            _cotacaoServiceMock.Setup(x => x.ValidarCotacaoExistenteAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            _cotacaoRepositoryMock
                .Setup(r => r.CadastrarCotacao(It.IsAny<CWCotacao>()))
                .ReturnsAsync(entidadeCotacao);

            _mapperMock
                .Setup(m => m.Map<CWCotacao>(dtoCotacao))
                .Returns(entidadeCotacao);

            var resultado = await _cotacaoService.CadastrarCotacao(dtoCotacao);

            Assert.NotNull(resultado);
            Assert.Equal(enumSituacaoRetorno.Sucesso, resultado.Status);
        }

        [Fact]
        public async Task CadastrarCotacao_DeveRetornarExcecao_QuandoCotacaoExiste()
        {
            var dtoCotacao = _cotacoesDTO.First();
            var entidadeCotacao = _cotacoes.First();

            _cotacaoServiceMock
                .Setup(x => x.ValidarCotacaoExistenteAsync(It.IsAny<int>()))
                .ReturnsAsync(false);

            _entidadeLeituraRepositoryMock
                .Setup(x => x.Consultar<CWCotacao>(It.IsAny<Expression<Func<CWCotacao, bool>>>()))
                .ReturnsAsync(new CWCotacao { nCdCotacao = 1 });

            _cotacaoRepositoryMock
                .Setup(r => r.CadastrarCotacao(It.IsAny<CWCotacao>()))
                .ReturnsAsync(entidadeCotacao);

            _mapperMock
                .Setup(m => m.Map<CWCotacao>(dtoCotacao))
                .Returns(entidadeCotacao);

            var excecao = await Assert.ThrowsAsync<ExcecaoCustomizada>(
                () => _cotacaoService.CadastrarCotacao(dtoCotacao));

            Assert.NotNull(excecao);
            Assert.Contains($"já criada", excecao.Message);
        }

        [Fact]
        public async Task EditarCotacao_DeveRetornarSucesso_QuandoCotacaoExiste()
        {
            var dtoCotacao = _cotacoesDTO.First();
            var entidadeCotacao = _cotacoes.First();

            _mapperMock
                .Setup(m => m.Map<CWCotacao>(dtoCotacao))
                .Returns(entidadeCotacao);

            _cotacaoRepositoryMock
                .Setup(r => r.EditarCotacao(It.IsAny<CWCotacao>()))
                .ReturnsAsync(entidadeCotacao);

            var resultado = await _cotacaoService.EditarCotacao(oDTOEditarCotacao);

            Assert.NotNull(resultado);
            Assert.True(resultado.Status == enumSituacaoRetorno.Sucesso);
            Assert.Contains("editada", resultado.Mensagem);
        }

        [Fact]
        public async Task EditarCotacao_DeveRetornarExcecao_QuandoCodigoNaoExiste()
        {

            var dtoCotacao = new DTOEditarCotacao() { CodigoCotacao = 0 };
            var entidadeCotacao = _cotacoes.First();

            _mapperMock
                .Setup(m => m.Map<CWCotacao>(dtoCotacao))
                .Returns(entidadeCotacao);

            _cotacaoRepositoryMock
                .Setup(r => r.EditarCotacao(It.IsAny<CWCotacao>()))
                .ReturnsAsync(entidadeCotacao);

            var excecao = await Assert.ThrowsAsync<ExcecaoCustomizada>(() => _cotacaoService.EditarCotacao(dtoCotacao));

            Assert.NotNull(excecao);
            Assert.Contains("obrigatório", excecao.Message);
        }

        [Fact]
        public async Task CompararCotacoes_DeveRetornarSempre_UmDTOCotacaoAnalise()
        {

            _scoreRepositoryMock.Setup(x => x.ObterScore(It.IsAny<int>()))
                .ReturnsAsync(cwScore);

            _produtoServiceMock.Setup(x => x.GarantirProdutosComuns(It.IsAny<List<CWCotacao>>()))
                .Returns(new List<int> { 1, 2 });

            _cotacaoRepositoryMock.Setup(r => r.PesquisarCotacoes())
                .ReturnsAsync(_cotacoes);

            var resultado = await _cotacaoService.CompararCotacoes(oDTOCotacaoAnalise);

            Assert.NotNull(resultado);
            Assert.IsType<List<DTOCotacaoRank>>(resultado);
        }

        #endregion
    }
}