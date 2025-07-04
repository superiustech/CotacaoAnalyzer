import React, { useEffect } from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import DashboardPage from './pages/DashboardPage';
import CotacaoPage from './pages/CotacaoPage';
import Layout from './components/layout/Layout';
import './styles/css/site.css';
import './styles/App.css';
import 'bootswatch/dist/darkly/bootstrap.min.css';
function App() {
    useEffect(() => {
        document.documentElement.lang = 'pt-BR';
        document.documentElement.charset = 'UTF-8';
    }, []);

    return (
        <Router>
            <Layout>
                <Routes>
                    <Route path="/dashboard" element={<DashboardPage />} />
                    <Route path="/cotacoes" element={<CotacaoPage />} />
                </Routes>
            </Layout>
        </Router>
    );
}

export default App;
