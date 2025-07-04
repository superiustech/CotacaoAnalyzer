import React from 'react';
import { NavLink } from 'react-router-dom';

const Header = () => {
    const navItems = [
        { path: "/dashboard", icon: "fa-home", text: "Dashboard" },
        { path: "/cotacoes", icon: "fa-chart-bar", text: "Cotações" },
    ];

    return (
        <nav className="navbar navbar-expand-lg navbar-dark bg-primary shadow-sm py-3">
            <div className="container-fluid px-4">
                <button
                    className="navbar-toggler border-0"
                    type="button"
                    data-bs-toggle="collapse"
                    data-bs-target="#navbarNav"
                    aria-controls="navbarNav"
                    aria-expanded="false"
                    aria-label="Toggle navigation">
                    <span className="navbar-toggler-icon"></span>
                </button>

                <div className="collapse navbar-collapse justify-content-between w-100" id="navbarNav">
                    <div className="d-none d-lg-flex">
                        <NavLink className="navbar-brand fw-bold fs-6 d-flex align-items-center" to="/dashboard">
                            <i className="fas fa-cubes me-2 fs-6"></i>
                            Cotação Analyzer
                        </NavLink>
                    </div>
                    <div className="d-none d-lg-flex">

                    <ul className="navbar-nav mx-auto gap-3 mb-2 mb-lg-0">
                        {navItems.map((item, index) => (
                            <li className="nav-item" key={index}>
                                <NavLink
                                    to={item.path}
                                    className={({ isActive }) =>
                                        `nav-link px-3 py-2 rounded-pill d-flex align-items-center transition 
                                        ${isActive ? 'bg-white text-primary fw-bold shadow-sm' : 'text-white-50'}`
                                    }
                                >
                                    <i className={`fas ${item.icon} me-2`}></i>
                                    {item.text}
                                </NavLink>
                            </li>
                        ))}
                    </ul>
                    </div>
                    <div className="d-none d-lg-flex">
                        <button className="btn btn-outline-light rounded-pill px-4 py-2 border-2 fw-medium transition hover-scale">
                            <i className="fas fa-sign-out-alt me-2"></i> Sair
                        </button>
                    </div>
                </div>
            </div>
        </nav>
    );
};

export default Header;
