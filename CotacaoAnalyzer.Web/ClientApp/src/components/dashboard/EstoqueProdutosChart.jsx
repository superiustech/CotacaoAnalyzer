import React from 'react';
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';

const EstoqueProdutosChart = ({ cotacoesRanqueadas }) => {
    return (
        <div style={{ width: '100%', height: '400px' }}>
            <h6 className="fw-bold fs-3 text-primary my-4 px-5 text-end">
                <i className="bi bi-speedometer2 me-2"></i> Produtos por estoques
            </h6>
            <ResponsiveContainer width="100%" height="100%">
                
            </ResponsiveContainer>
        </div>
    );
};

export default EstoqueProdutosChart;