import Loading from '../components/ui/Loading';
import FlashMessage from '../components/ui/FlashMessage';
import CompararCotacao from '../components/cotacao/CompararCotacao';
import { CotacaoService } from '../services/CotacaoService';

const DashboardPage = () => {
    const { loading, error, success, mensagem, cotacoes, produtos, pesos, modal, editarCotacao, cadastrarCotacao, incluirProduto, compararCotacoes, carregarCotacoes, abrirModal, fecharModal } = CotacaoService();
    const codigosCotacoes = cotacoes.slice(0, 6).map(c => c.codigoCotacao);

    return (
        <div className="container">
            {success && <FlashMessage message={mensagem} type="success" duration={3000} />}
            {error && <FlashMessage message={mensagem} type="error" duration={3000} />}
            {loading ? (<Loading show={true} />) : (
                <>
                    <h1 className="fw-bold text-white"> <i className="bi bi-speedometer2"></i> Dashboard</h1>  
                <hr className="mb-4"></hr>
                <CompararCotacao 
                    onSubmit={compararCotacoes}
                    onCancel={fecharModal}
                    pesos={pesos}
                    codigosCotacoes={codigosCotacoes} />
                </>
            )}
        </div>
    );
};

export default DashboardPage;
