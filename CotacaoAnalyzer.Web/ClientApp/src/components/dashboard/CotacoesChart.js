import React from 'react';
import {
    BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer,
    PieChart, Pie, Cell
} from 'recharts';

const CotacoesChart = ({ cotacoesRanqueadas }) => {

    const dataValorTotal = cotacoesRanqueadas.map(c => ({
        descricao: c.descricao,
        valorTotal: c.valorTotal
    }));

    const dataScore = cotacoesRanqueadas.map(c => ({
        descricao: c.descricao,
        score: c.score
    }));

    const dataPrazoMedio = cotacoesRanqueadas.map(c => {
        const totalPrazo = c.itens.reduce((acc, item) => acc + item.prazoEntrega, 0);
        const mediaPrazo = c.itens.length > 0 ? totalPrazo / c.itens.length : 0;
        return {
            descricao: c.descricao,
            prazoMedio: Number(mediaPrazo.toFixed(2))
        };
    });

    const totalGeral = cotacoesRanqueadas.reduce((acc, c) => acc + c.valorTotal, 0);
    const dataPie = cotacoesRanqueadas.map(c => ({
        descricao: c.descricao,
        percentual: Number(((c.valorTotal / totalGeral) * 100).toFixed(2))
    }));

    const COLORS = ['#0088FE', '#00C49F', '#FFBB28', '#FF8042', '#AA49FF', '#FF4D8D'];

    return (
        <div className="container my-5 bg-dark text-light rounded p-4">
            <h6 className="fw-bold fs-3 text-white my-4 text-end">
                <i className="bi bi-speedometer2 me-2"></i> Comparativo de Cotações
            </h6>

            <div className="row g-4">
                <div className="col-12 col-md-6">
                    <div className="p-3 shadow rounded bg-secondary" style={{ height: '400px' }}>
                        <ResponsiveContainer>
                            <BarChart data={dataValorTotal} layout="vertical" margin={{ top: 20, right: 30, left: 100, bottom: 20 }}>
                                <CartesianGrid strokeDasharray="3 3" stroke="#ccc" />
                                <XAxis type="number" stroke="#fff" />
                                <YAxis dataKey="descricao" type="category" stroke="#fff" />
                                <Tooltip contentStyle={{ backgroundColor: '#333', color: '#fff' }} />
                                <Legend />
                                <Bar dataKey="valorTotal" fill="#8884d8" name="Valor Total (R$)" />
                            </BarChart>
                        </ResponsiveContainer>
                    </div>
                </div>

                <div className="col-12 col-md-6">
                    <div className="p-3 shadow rounded bg-secondary" style={{ height: '400px' }}>
                        <ResponsiveContainer>
                            <BarChart data={dataScore} margin={{ top: 20, right: 30, left: 20, bottom: 20 }}>
                                <CartesianGrid strokeDasharray="3 3" stroke="#ccc" />
                                <XAxis dataKey="descricao" stroke="#fff" />
                                <YAxis stroke="#fff" />
                                <Tooltip contentStyle={{ backgroundColor: '#333', color: '#fff' }} />
                                <Legend />
                                <Bar dataKey="score" fill="#82ca9d" name="Score" />
                            </BarChart>
                        </ResponsiveContainer>
                    </div>
                </div>

                <div className="col-12 col-md-6">
                    <div className="p-3 shadow rounded bg-secondary" style={{ height: '400px' }}>
                        <ResponsiveContainer>
                            <BarChart data={dataPrazoMedio} margin={{ top: 20, right: 30, left: 20, bottom: 20 }}>
                                <CartesianGrid strokeDasharray="3 3" stroke="#ccc" />
                                <XAxis dataKey="descricao" stroke="#fff" />
                                <YAxis stroke="#fff" />
                                <Tooltip contentStyle={{ backgroundColor: '#333', color: '#fff' }} />
                                <Legend />
                                <Bar dataKey="prazoMedio" fill="#ffc658" name="Prazo Médio (dias)" />
                            </BarChart>
                        </ResponsiveContainer>
                    </div>
                </div>

                <div className="col-12 col-md-6">
                    <div className="p-3 shadow rounded bg-secondary" style={{ height: '400px' }}>
                        <ResponsiveContainer>
                            <PieChart>
                                <Pie
                                    data={dataPie}
                                    dataKey="percentual"
                                    nameKey="descricao"
                                    cx="50%"
                                    cy="50%"
                                    outerRadius={120}
                                    label={({ descricao, percentual }) => `${descricao}: ${percentual}%`}
                                >
                                    {dataPie.map((entry, index) => (
                                        <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                                    ))}
                                </Pie>
                                <Tooltip contentStyle={{ backgroundColor: '#333', color: '#fff' }} />
                                <Legend />
                            </PieChart>
                        </ResponsiveContainer>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default CotacoesChart;
