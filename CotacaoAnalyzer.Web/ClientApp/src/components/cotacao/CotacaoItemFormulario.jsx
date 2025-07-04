import React, { useState } from "react";
import { Form, Button, Table } from "react-bootstrap";

const CotacaoItemFormulario = ({ onSubmit, onCancel, cotacao, produtos }) => {
    const [itens, setItens] = useState([]);
    const [formData, setFormData] = useState({ codigoCotacaoItem: 0, sequencial: 0, prazoEntrega: 0, valorProposto: 0, produto: null
    });

    const handleChange = (e) => {
        const { name, value, type, checked } = e.target;
        setFormData({
            ...formData,
            [name]: type === 'radio' ? checked : value
        });
    };

    const handleChangeProdutos = (e) => {
        const value = e.target.value;

        if (value === "") {
            setFormData({
                ...formData,
                produto: null,
                valorProposto: 0,
                prazoEntrega: 0
            });
        } else {
            const selectedProduto = produtos.find(p => p.codigoProduto == value);
            if (selectedProduto) {
                setFormData({
                    ...formData,
                    produto: selectedProduto,
                    valorProposto: selectedProduto.valorProduto
                });
            }
        }
    };

    const adicionarItem = () => {
        if (!formData.produto) {
            alert("Selecione um produto antes de adicionar!");
            return;
        }

        const novoItem = {
            ...formData,
            sequencial: itens.length + 1
        };

        setItens([...itens, novoItem]);

        setFormData({
            codigoCotacaoItem: 0,
            sequencial: 0,
            prazoEntrega: 0,
            valorProposto: 0,
            produto: null
        });
    };

    const removerItem = (index) => {
        const novosItens = itens.filter((_, i) => i !== index);
        const itensReordenados = novosItens.map((item, i) => ({
            ...item,
            sequencial: i + 1
        }));
        setItens(itensReordenados);
    };

    const handleSubmit = (e) => {
        e.preventDefault();

        if (itens.length === 0) {
            alert("Adicione pelo menos um item à cotação!");
            return;
        }

        const valorTotal = itens.reduce((total, item) => total + item.valorProposto, 0);

        const dadosEnvio = {
            codigoCotacao: cotacao.codigoCotacao,
            itens: itens
        };

        onSubmit(dadosEnvio);
    };

    return (
        <Form onSubmit={handleSubmit} className="p-2">
            <Form.Group className="mb-3" controlId="produtosSelect">
                <Form.Label>Selecione um Produto</Form.Label>
                <Form.Select name="produtosSelect" value={formData.produto?.codigoProduto || ""} onChange={handleChangeProdutos}>
                    <option value="">Selecione um produto</option>
                    {produtos.map((produto) => (
                        <option key={produto.codigoProduto} value={produto.codigoProduto}>
                            {produto.nomeProduto} - R$ {produto.valorProduto}
                        </option>
                    ))}
                </Form.Select>
            </Form.Group>

            <div className="row g-3 mb-3">
                <div className="col-md-3">
                    <Form.Group controlId="codigoProduto">
                        <Form.Label>Código</Form.Label>
                        <Form.Control type="text" value={formData.produto?.codigoProduto || ""} disabled/>
                    </Form.Group>
                </div>
                <div className="col-md-3">
                    <Form.Group controlId="codigoSKU">
                        <Form.Label>SKU</Form.Label>
                        <Form.Control type="text" value={formData.produto?.codigoSKU || ""} disabled/>
                    </Form.Group>
                </div>
                <div className="col-md-3">
                    <Form.Group controlId="nomeProduto">
                        <Form.Label>Nome</Form.Label>
                        <Form.Control type="text" value={formData.produto?.nomeProduto || ""} disabled/>
                    </Form.Group>
                </div>
                <div className="col-md-1">
                    <Form.Group controlId="prazoEntrega">
                        <Form.Label>Prazo</Form.Label>
                        <Form.Control type="number" value={formData.prazoEntrega || 0} onChange={handleChange} name="prazoEntrega" />
                    </Form.Group>
                </div>
                <div className="col-md-2">
                    <Form.Group controlId="valorProposto">
                        <Form.Label>Valor (R$)</Form.Label>
                        <Form.Control type="text" value={formData.valorProposto || ""} onChange={handleChange} name="valorProposto"/>
                    </Form.Group>
                </div>
            </div>

            <div className="mb-3">
                <Button variant="primary" onClick={adicionarItem} disabled={!formData.produto}>Adicionar Item</Button>
            </div>
            {itens.length > 0 && (
                <>
                    <h5 className="mt-4 mb-3">Itens da Cotação</h5>
                    <div className="table-responsive">
                        <Table striped bordered hover variant="dark" className="align-middle">
                            <thead className="table-dark">
                                <tr>
                                    <th width="5%">#</th>
                                    <th width="10%">Código</th>
                                    <th width="15%">SKU</th>
                                    <th>Produto</th>
                                    <th width="15%" className="text-end">Valor (R$)</th>
                                    <th width="10%" className="text-center">Ações</th>
                                </tr>
                            </thead>
                            <tbody>
                                {itens.map((item, index) => (
                                    <tr key={index}>
                                        <td className="fw-bold">{item.sequencial}</td>
                                        <td>{item.produto.codigoProduto}</td>
                                        <td><span className="badge bg-secondary"> {item.produto.codigoSKU}</span></td>
                                        <td>
                                            <div className="d-flex flex-column">
                                                <span className="fw-semibold">{item.produto.nomeProduto}</span>
                                                <small className="text-muted">Prazo: {item.prazoEntrega} dias</small>
                                            </div>
                                        </td>
                                        <td className="text-end fw-bold text-success"> {item.valorProposto.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL'})}</td>
                                        <td className="text-center">
                                            <Button variant="outline-danger" size="sm" onClick={() => removerItem(index)} title="Remover item">
                                                <i className="fa fa-trash"></i>
                                            </Button>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                            <tfoot>
                                <tr className="table-active">
                                    <td colSpan="4" className="text-end fw-bold">Total:</td>
                                    <td className="text-end fw-bold text-white">
                                        {itens.reduce((total, item) => total + item.valorProposto, 0).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL'})}
                                    </td>
                                    <td></td>
                                </tr>
                            </tfoot>
                        </Table>
                    </div>
                </>
            )}

            <div className="d-flex justify-content-end mt-3">
                <Button variant="secondary" onClick={onCancel} className="me-2">
                    Cancelar
                </Button>
                <Button variant="primary" type="submit" disabled={itens.length === 0}>
                    Salvar Cotação
                </Button>
            </div>
        </Form>
    );
};

export default CotacaoItemFormulario;