import React, { useState, useEffect } from "react";
import { Form, Button } from "react-bootstrap";

const CotacaoFormulario = ({ onSubmit, onCancel, cotacao }) => {
    const [formData, setFormData] = useState({ codigoCotacao:0, descricao: '', data: null, freteIncluso: false });
    
    const handleChange = (e) => {
        const { name, value, type, checked } = e.target;
        setFormData({ ...formData, [name]: type === 'radio' ? checked : value });
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        onSubmit({ ...formData, data: new Date(formData.data).toISOString() });
    };

    useEffect(() => {
        if (cotacao) {
            setFormData({
                ...cotacao, data: cotacao.data ? cotacao.data.split('T')[0] : ''
            });}
    }, [cotacao]); 

    return (
        <Form onSubmit={handleSubmit} className="p-2">
            <Form.Group className="mb-3" controlId="descricao">
                <Form.Label>Código</Form.Label>
                <Form.Control type="text" name="codigoCotacao" value={formData.codigoCotacao} onChange={handleChange} disabled/>
            </Form.Group>
            <Form.Group className="mb-3" controlId="descricao">
                <Form.Label>Descrição</Form.Label>
                <Form.Control type="text" placeholder="Descrição..." name="descricao" value={formData.descricao} onChange={handleChange} />
            </Form.Group>
            <Form.Group className="mb-3" controlId="data">
                <Form.Label>Data</Form.Label>
                <Form.Control type="date" name="data" value={formData.data} onChange={handleChange} />
            </Form.Group>
            <Form.Group className="mb-3" controlId="freteIncluso">
                <Form.Label>Frete incluso</Form.Label>
                <div className="d-flex flex-row bd-highlight mb-3">
                    <Form.Check type="radio" label="Sim" value="true" name="freteIncluso"  onChange={() => setFormData({ ...formData, freteIncluso: true })} className="me-2" checked={formData.freteIncluso === true}/>
                    <Form.Check type="radio" label="Não" value="false" name="freteIncluso" onChange={() => setFormData({ ...formData, freteIncluso: false })} className="me-2" checked={formData.freteIncluso === false}
                    />
                </div>
            </Form.Group>

            <div className="d-flex justify-content-end mt-3">
                <Button variant="secondary" onClick={onCancel} className="me-2">Cancelar</Button>
                <Button variant="primary" type="submit">Salvar</Button>
            </div>
        </Form>
    );
};

export default CotacaoFormulario;
