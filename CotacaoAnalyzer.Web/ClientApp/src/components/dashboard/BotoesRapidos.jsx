import { useNavigate } from 'react-router-dom';
const BotoesRapidos = () => {
    const navigate = useNavigate();

    const configOptions = [
        { label: "Cadastrar Produtos", icon: "fas fa-box-open", path: "/administrador/cadastrar-produto"},
        { label: "Visualizar Produtos", icon: "fas fa-box-open", path: "/administrador/produtos"},
        { label: "Seus estoques", icon: "fas fa-warehouse", path: "/administrador/estoques"},
        { label: "Seus Revendedores", icon: "fas fa-users", path: "/administrador/revendedores"},
    ];

    return (
        <div>
            <h6 className="fw-bold fs-3 text-primary my-4 px-5 text-start">
                <i className="bi bi-speedometer2 me-2"></i> Ações rápidas
            </h6>
            <div className="d-flex flex-column gap-3">
                {configOptions.map((option, index) => (
                        <div
                            key={index}
                            role="button"
                            className="card shadow-sm border-0 p-3"
                            onClick={() => navigate(option.path)}
                            style={{
                                cursor: 'pointer',
                                transition: 'transform 0.2s',
                                minHeight: '80px'
                            }}
                            onMouseEnter={e => e.currentTarget.style.transform = 'scale(1.02)'}
                            onMouseLeave={e => e.currentTarget.style.transform = 'scale(1)'}>
                            <div className="d-flex align-items-center">
                                <i className={`${option.icon} fs-4 text-primary me-3`}></i>
                                <span className="fw-semibold">{option.label}</span>
                            </div>
                        </div>
                   
                ))}
            </div>
        </div>
    );
};

export default BotoesRapidos;

