import { useState, useEffect, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import apiConfig from '../Api';
import axios from 'axios';

export const CotacaoService = () => {
    const [state, setState] = useState({ loading: false, error: null, success: false, mensagem: '', cotacoes: [], produtos: [], pesos: [], modal: { open: false, tipo: null, data: null, chave: [], referencia: null} });

    // Callbacks 

    const carregarCotacoes = useCallback(async () => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const response = await axios.get(`${apiConfig.cotacao.baseURL}${apiConfig.cotacao.endpoints.pesquisarCotacoes}`)
            setState(prev => ({ ...prev, cotacoes: response.data, loading: false }));
        }
        catch (err) {
            setState(prev => ({ ...prev, error: true, mensagem: "Erro ao carregar os dados", loading: false }));
        }
    }, []);

    const carregarProdutos = useCallback(async () => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const response = await axios.get(`${apiConfig.produto.baseURL}${apiConfig.produto.endpoints.pesquisarProdutos}`)
            setState(prev => ({ ...prev, produtos: response.data, loading: false }));
        }
        catch (err) {
            setState(prev => ({ ...prev, error: true, mensagem: "Erro ao carregar os dados", loading: false }));
        }
    }, []);

    const carregarPesos = useCallback(async () => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const response = await axios.get(`${apiConfig.peso.baseURL}${apiConfig.peso.endpoints.pesquisarPeso}`)
            setState(prev => ({ ...prev, pesos: response.data, loading: false }));
        }
        catch (err) {
            setState(prev => ({ ...prev, error: true, mensagem: "Erro ao carregar os dados", loading: false }));
        }
    }, []);

    // --- Fim callbacks

    // Funções

    const cadastrarCotacao = async (formData) => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const dadosEnvio = {
                codigoCotacao: 0,
                descricao: formData.descricao,
                data: formData.data,
                freteIncluso: formData.freteIncluso,
                valorTotal: 0,
                itens: []
            };

            const response = await axios.post(`${apiConfig.cotacao.baseURL}${apiConfig.cotacao.endpoints.cadastrarCotacao}`,
                JSON.stringify(dadosEnvio), {
                    headers: { 'Content-Type': 'application/json',
                }
            });
            carregarCotacoes();
            fecharModal();
            setState(prev => ({ ...prev, success: true, mensagem: response.data?.mensagem || "Operação realizada com sucesso", loading: false }));
        } catch (err) {
            const errorMessage = err.response?.data?.mensagem || err.message || "Erro na comunicação com o servidor";
            setState(prev => ({ ...prev, error: true, mensagem: errorMessage, loading: false }));
        }
    };

    const editarCotacao = async (formData) => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const dadosEnvio = {
                codigoCotacao: formData.codigoCotacao,
                descricao: formData.descricao,
                data: formData.data,
                freteIncluso: formData.freteIncluso,
                valorTotal: 0
            };

            const response = await axios.put(`${apiConfig.cotacao.baseURL}${apiConfig.cotacao.endpoints.editarCotacao}`,
                JSON.stringify(dadosEnvio), {
                headers: {
                    'Content-Type': 'application/json',
                }
            });
            carregarCotacoes();
            fecharModal();
            setState(prev => ({ ...prev, success: true, mensagem: response.data?.mensagem || "Operação realizada com sucesso", loading: false }));
        } catch (err) {
            const errorMessage = err.response?.data?.mensagem || err.message || "Erro na comunicação com o servidor";
            setState(prev => ({ ...prev, error: true, mensagem: errorMessage, loading: false }));
        }
    };

    const incluirProduto = async (formData) => {
        setState(prev => ({ ...prev, loading: true }));
        try {
            const response = await axios.post(`${apiConfig.cotacao.baseURL}${apiConfig.cotacao.endpoints.adicionarItens}`,
                JSON.stringify(formData), {
                headers: {
                    'Content-Type': 'application/json',
                }
            });
            carregarCotacoes();
            fecharModal();
            setState(prev => ({ ...prev, success: true, mensagem: response.data?.mensagem || "Operação realizada com sucesso", loading: false }));
        } catch (err) {
            const errorMessage = err.response?.data?.mensagem || err.message || "Erro na comunicação com o servidor";
            setState(prev => ({ ...prev, error: true, mensagem: errorMessage, loading: false }));
        }
    };

    // No componente pai:
    const compararCotacoes = async (formData) => {
        try {
            const response = await axios.post(
                `${apiConfig.cotacao.baseURL}${apiConfig.cotacao.endpoints.compararCotacoes}`,
                formData, // Remove JSON.stringify - o Axios já faz isso
                {
                    headers: { 'Content-Type': 'application/json' }
                }
            );

            // Retorne APENAS os dados, sem mexer no estado
            return response.data.cotacoesRanqueadas || response.data;
        } catch (err) {
            console.error("Erro na comparação:", err);
            throw err;
        }
    };

    // --- Fim funções

    // Modais

    const abrirModal = (nomeModal, data) => {
        if (nomeModal === 'cotacao') cadastrarCotacaoModal();
        else if (nomeModal === 'editar') editarCotacaoModal(data);
        else if (nomeModal === 'incluirProduto') incluirProdutoModal(data);
        else if (nomeModal === 'compararCotacoes') compararCotacoesModal(data);
        else fecharModal();
    }

    const fecharModal = () => { setState(prev => ({ ...prev, modal: { open: false, tipo: null, data: null } })); };

    const incluirProdutoModal = async (codigoCotacao) => {
        setState(prev => ({ ...prev, loading: true }));
        setState(prev => ({ ...prev, modal: { open: true, tipo: 'incluirProduto', referencia: state.cotacoes.find((e) => e.codigoCotacao == codigoCotacao) } }));
        setState(prev => ({ ...prev, loading: false }));
    };

    const editarCotacaoModal = async (codigoCotacao) => {
        setState(prev => ({ ...prev, loading: true }));
        setState(prev => ({ ...prev, modal: { open: true, tipo: 'editar', referencia: state.cotacoes.find((e) => e.codigoCotacao == codigoCotacao) } }));
        setState(prev => ({ ...prev, loading: false }));
    };

    const compararCotacoesModal = async (codigosCotacao) => {
        setState(prev => ({ ...prev, loading: true }));
        await carregarPesos(); 
        setState(prev => ({ ...prev, modal: { open: true, tipo: 'compararCotacoes', chave: state.pesos, referencia: codigosCotacao }}));
        setState(prev => ({ ...prev, loading: false }));
    };

    const cadastrarCotacaoModal = async () => {
        setState(prev => ({ ...prev, loading: true }));
        setState(prev => ({ ...prev, modal: { open: true, tipo: 'cotacao' } }));
        setState(prev => ({ ...prev, loading: false }));
    };

    // --- Fim modais 

    useEffect(() => {
        carregarCotacoes();
        carregarProdutos();
        carregarPesos();
    }, []);

    return { ...state, editarCotacao, cadastrarCotacao, incluirProduto, compararCotacoes, carregarCotacoes, abrirModal, fecharModal };
};
