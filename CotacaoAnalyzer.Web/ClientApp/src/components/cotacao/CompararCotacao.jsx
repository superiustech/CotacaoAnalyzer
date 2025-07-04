import React, { useState } from "react";
import { Form, Button, Collapse } from "react-bootstrap";
import apiConfig from '../../Api';
import axios from 'axios';
import FlashMessage from '../../components/ui/FlashMessage';
import CotacoesChart from '../dashboard/CotacoesChart';

const CompararCotacao = ({ onSubmit, onCancel, pesos, codigosCotacoes }) => {
    const [formData, setFormData] = useState({ codigoScore: 0, pesoValor: 0, pesoFreteIncluso: 0, pesoPrazoEntrega: 0 });
    const [inputsDisabled, setInputsDisabled] = useState(false);
    const [success, setSuccess] = useState(false);
    const [error, setError] = useState(false);
    const [mensagem, setMensagem] = useState('');
    const [cotacoesRanqueadas, setCotacoesRanqueadas] = useState(null);
    const [showFilters, setShowFilters] = useState(true); // novo estado

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData({
            ...formData,
            [name]: value
        });
    };

    const handleChangePesos = (e) => {
        const value = e.target.value;

        if (value === "") {
            setFormData({ codigoScore: 0, pesoValor: 0, pesoFreteIncluso: 0, pesoPrazoEntrega: 0 });
            setInputsDisabled(false);
        } else {
            const selectedPeso = pesos.find(p => p.codigoScore == value);
            if (selectedPeso) {
                setFormData({
                    codigoScore: selectedPeso.codigoScore,
                    pesoValor: selectedPeso.pesoValor,
                    pesoFreteIncluso: selectedPeso.pesoFreteIncluso,
                    pesoPrazoEntrega: selectedPeso.pesoPrazoEntrega
                });
                setInputsDisabled(true);
            }
        }
        setMensagem('');
        setSuccess(false);
        setError(false);
    };

    const limparFormulario = () => {
        setFormData({ codigoScore: 0, pesoValor: 0, pesoFreteIncluso: 0, pesoPrazoEntrega: 0 });
        setInputsDisabled(false);
        setMensagem('');
        setSuccess(false);
        setError(false);
    };

    const adicionarPeso = async () => {
        if (inputsDisabled) {
            setMensagem("Selecione o modo de novo peso para adicionar.");
            setError(true);
            setSuccess(false);
            return;
        }

        try {
            var response = await axios.post(
                `${apiConfig.peso.baseURL}${apiConfig.peso.endpoints.cadastrarPeso}`,
                formData,
                { headers: { 'Content-Type': 'application/json' } }
            );

            pesos.push(response.data.id);
            setSuccess(true);
            setError(false);
            setMensagem("Peso cadastrado com sucesso!");
            limparFormulario();
        } catch (err) {
            const errorMessage = err.response?.data?.mensagem || err.message || "Erro ao cadastrar peso";
            setMensagem(errorMessage);
            setError(true);
            setSuccess(false);
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const codigos = Array.isArray(codigosCotacoes)
                ? codigosCotacoes
                : typeof codigosCotacoes === 'string'
                    ? codigosCotacoes.split(',').map(Number)
                    : [];

            const resultado = await onSubmit({
                codigosCotacoes: codigos,
                codigoScore: formData.codigoScore
            });

            setCotacoesRanqueadas([...resultado]);
            setSuccess(true);
            setMensagem("Comparação realizada com sucesso!");

        } catch (error) {
            setError(true);
            setMensagem(error.response?.data?.mensagem || "Erro na comparação");
        }
    };

    return (
        <>
            {(success || error) && (
                <FlashMessage
                    message={mensagem}
                    type={success ? "success" : "error"}
                    duration={3000}
                />
            )}

            <div className="d-flex justify-content-between align-items-center mb-3">
                <Button
                    variant="outline-light"
                    size="sm"
                    onClick={() => setShowFilters(!showFilters)}
                >
                    {showFilters ? (
                        <><i className="fa fa-eye-slash me-2"></i>Ocultar filtros</>
                    ) : (
                        <><i className="fa fa-eye me-2"></i>Mostrar filtros</>
                    )}
                </Button>
            </div>

            <Collapse in={showFilters}>
                <div>
                    <Form onSubmit={handleSubmit} className="p-2">
                        <Form.Group className="mb-3">
                            <Form.Label>Peso</Form.Label>
                            <Form.Select value={formData.codigoScore || ""} onChange={handleChangePesos}>
                                <option value="">Selecione um peso</option>
                                {pesos.map((peso) => (
                                    <option key={peso.codigoScore} value={peso.codigoScore}>
                                        Peso valor: {peso.pesoValor} - Peso frete incluso: {peso.pesoFreteIncluso} - Peso prazo entrega: {peso.pesoPrazoEntrega}
                                    </option>
                                ))}
                            </Form.Select>
                        </Form.Group>

                        <div className="row g-3 mb-3">
                            <div className="col-md-3">
                                <Form.Group>
                                    <Form.Label>Código</Form.Label>
                                    <Form.Control type="text" value={formData.codigoScore || ""} disabled />
                                </Form.Group>
                            </div>
                            <div className="col-md-2">
                                <Form.Group>
                                    <Form.Label>P. Valor</Form.Label>
                                    <Form.Control
                                        type="number"
                                        name="pesoValor"
                                        value={formData.pesoValor}
                                        onChange={handleChange}
                                        disabled={inputsDisabled}
                                    />
                                </Form.Group>
                            </div>
                            <div className="col-md-2">
                                <Form.Group>
                                    <Form.Label>P. Frete</Form.Label>
                                    <Form.Control
                                        type="number"
                                        name="pesoFreteIncluso"
                                        value={formData.pesoFreteIncluso}
                                        onChange={handleChange}
                                        disabled={inputsDisabled}
                                    />
                                </Form.Group>
                            </div>
                            <div className="col-md-2">
                                <Form.Group>
                                    <Form.Label>P. Prazo</Form.Label>
                                    <Form.Control
                                        type="number"
                                        name="pesoPrazoEntrega"
                                        value={formData.pesoPrazoEntrega}
                                        onChange={handleChange}
                                        disabled={inputsDisabled}
                                    />
                                </Form.Group>
                            </div>
                        </div>

                        <div className="mb-3 d-flex gap-2">
                            <Button variant="success" onClick={adicionarPeso} disabled={inputsDisabled}>
                                <i className="fa fa-plus me-2"></i>Adicionar novo peso
                            </Button>
                            <Button variant="warning" onClick={limparFormulario}>
                                <i className="fa fa-eraser me-2"></i>Limpar
                            </Button>
                        </div>

                        <div className="d-flex justify-content-end mt-3">
                            <Button variant="secondary" onClick={onCancel} className="me-2">
                                Cancelar
                            </Button>
                            <Button variant="primary" type="submit">
                                Comparar cotações
                            </Button>
                        </div>
                    </Form>
                </div>
            </Collapse>

            {cotacoesRanqueadas && cotacoesRanqueadas.length > 0 ? (
                <CotacoesChart cotacoesRanqueadas={cotacoesRanqueadas} />
            ) : (
                <p>Nenhuma cotação para mostrar.</p>
            )}
        </>
    );
};

export default CompararCotacao;
