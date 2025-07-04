import config from './config';

const { baseApiUrl } = config;

export default {
    cotacao: {
        baseURL: `${baseApiUrl}/Cotacao`, 
        endpoints: {
            pesquisarCotacoes: "/Cotacoes",
            compararCotacoes: '/CompararCotacoes',
            cadastrarCotacao: "/CadastrarCotacao",
            editarCotacao: "/EditarCotacao",
            adicionarItens: "/AdicionarItens"
        }
    },
    produto: {
        baseURL: `${baseApiUrl}/Produto`,
        endpoints: {
            pesquisarProdutos: "/Produtos",
            cadastrarProduto: "/CadsatrarProduto",
            editarProduto: "/EditarProduto"
        }
    },
    peso: {
        baseURL: `${baseApiUrl}/Score`,
        endpoints: {
            pesquisarPeso: "/Scores",
            cadastrarPeso: "/CadastrarScore",
            editarPeso: "/EditarScore"
        }
    }
};