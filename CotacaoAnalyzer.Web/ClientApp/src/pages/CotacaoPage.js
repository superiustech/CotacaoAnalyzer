import Loading from '../components/ui/Loading';
import FlashMessage from '../components/ui/FlashMessage';
import CotacaoTable from '../components/cotacao/CotacaoTable';
import CotacaoFormulario from '../components/cotacao/CotacaoFormulario';
import CotacaoItemFormulario from '../components/cotacao/CotacaoItemFormulario';
import CompararCotacao from '../components/cotacao/CompararCotacao';
import { CotacaoService } from '../services/CotacaoService';
import { Modal } from "react-bootstrap";
import React, { useState, useEffect } from 'react';

const DashboardPage = () => {
    const { loading, error, success, mensagem, cotacoes, produtos, pesos, modal, editarCotacao, cadastrarCotacao, incluirProduto, compararCotacoes, carregarCotacoes, abrirModal, fecharModal} = CotacaoService();
    const [selected, setSelected] = useState([]);

    return (
        <div className="container">
            {success && <FlashMessage message={mensagem} type="success" duration={3000} />}
            {error && <FlashMessage message={mensagem} type="error" duration={3000} />}
            {loading ? (<Loading show={true} />) : (
                <>
                    <h1 className="fw-bold display-5 text-white mb-5">
                        <i className="fas fa-file-invoice-dollar me-3"></i> Cotações
                    </h1>

                    <div className="mt-4 mb-3 d-flex justify-content-between align-items-center">
                        <div className="d-flex align-items-center">
                            <button className="btn btn-primary me-2" onClick={() => abrirModal('cotacao', null)}>
                                <i className="fas fa-plus me-2"></i>Incluir
                            </button>
                            <button className="btn btn-secondary me-2" disabled={selected.length !== 1} onClick={() => {
                                const codigo = selected.map(item => item.codigoCotacao);
                                abrirModal('editar', codigo);
                            }}>
                                <i className="fas fa-pen me-2"></i>Editar
                            </button>
                            <button className="btn btn-success me-2" disabled={selected.length !== 1} onClick={() => {
                                const codigo = selected.map(item => item.codigoCotacao);
                                abrirModal('incluirProduto', codigo);
                            }}>
                                <i className="fas fa-cart-plus me-2"></i>Incluir Produtos
                            </button>
                            <button className="btn btn-info me-2" disabled={selected.length === 0} onClick={() => {
                                const codigos = selected.map(item => item.codigoCotacao).join(",");
                                abrirModal('compararCotacoes', codigos);
                            }}>
                                <i className="fas fa-chart-simple me-2"></i>Comparar cotações
                            </button>
                        </div>
                        <div>
                            <button onClick={carregarCotacoes} className="btn btn-outline-secondary" disabled={loading}> {loading ? 'Atualizando...' : 'Atualizar Lista'}</button>
                        </div>
                    </div>

                    <CotacaoTable cotacoes={cotacoes} loading={loading} setSelected={setSelected} />

                    {modal.open && (
                        <Modal show={modal.open} onHide={fecharModal} centered size={modal.tipo === 'incluirProduto' || modal.tipo === 'compararCotacoes' ? "xl" : "md"} animation={true}>
                            <Modal.Header closeButton>
                                <Modal.Title>
                                    {modal.tipo === 'cotacao' && 'Nova cotação'}
                                    {modal.tipo === 'editar' && 'Editar Cotação'}
                                    {modal.tipo === 'incluirProduto' && 'Novo Item'}
                                    {modal.tipo === 'compararCotacoes' && 'Comparar cotações'}
                                </Modal.Title>
                            </Modal.Header>
                            <Modal.Body>
                                {modal.tipo === 'cotacao' && (
                                    <CotacaoFormulario
                                        onSubmit={cadastrarCotacao}
                                        onCancel={fecharModal}
                                    />
                                )}
                                {modal.tipo === 'editar' && (
                                    <CotacaoFormulario
                                        onSubmit={editarCotacao}
                                        onCancel={fecharModal}
                                        cotacao={modal.referencia}
                                    />
                                )}
                                {modal.tipo === 'incluirProduto' && (
                                    <CotacaoItemFormulario
                                        onSubmit={incluirProduto}
                                        onCancel={fecharModal}
                                        cotacao={modal.referencia}
                                        produtos={produtos}
                                    />
                                )}
                                {modal.tipo === 'compararCotacoes' && (
                                    <CompararCotacao
                                        onSubmit={compararCotacoes}
                                        onCancel={fecharModal}
                                        pesos={pesos}
                                        codigosCotacoes={modal.referencia}
                                    />
                                )}
                            </Modal.Body>
                        </Modal>
                    )}
                </>
            )}
        </div>
    );
};

export default DashboardPage;
