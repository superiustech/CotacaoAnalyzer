import React, { useState, useEffect } from 'react';

const FlashMessage = ({ message, details, type = 'error', duration = 3000 }) => {
    const [visible, setVisible] = useState(false);
    const [fading, setFading] = useState(false);

    useEffect(() => {
        if (message) {
            setVisible(true);
            setFading(false);

            // Inicia o fade out pouco antes de desaparecer
            const fadeOutTimer = setTimeout(() => setFading(true), duration - 500);
            // Esconde o componente após o fade out
            const hideTimer = setTimeout(() => setVisible(false), duration);

            return () => {
                clearTimeout(fadeOutTimer);
                clearTimeout(hideTimer);
            };
        }
    }, [message, duration]);

    if (!visible) return null;

    const colors = {
        error: {
            bg: 'var(--bs-danger)',
            text: 'var(--bs-white)',
            icon: 'bi-exclamation-octagon-fill'
        },
        success: {
            bg: 'var(--bs-success)',
            text: 'var(--bs-white)',
            icon: 'bi-check-circle-fill'
        },
        warning: {
            bg: 'var(--bs-warning)',
            text: 'var(--bs-dark)',
            icon: 'bi-exclamation-triangle-fill'
        },
        info: {
            bg: 'var(--bs-info)',
            text: 'var(--bs-white)',
            icon: 'bi-info-circle-fill'
        }
    };

    return (
        <div id="divMessage" style={{
            display: 'block',
            opacity: fading ? 0 : 1,
            transition: 'opacity 0.5s ease-in-out'
        }}>
            <div className="flash-message" style={{
                padding: '10px',
                background: colors[type].bg,
                borderLeft: `4px solid ${colors[type].border}`,
                marginBottom: '10px'
            }}>
                <strong>{message}</strong>
                {details && <div style={{ marginTop: '5px' }}>{details}</div>}
                <p style={{ margin: '5px 0 0 0', fontSize: '0.8em', color: '#fff' }}>
                    Esta mensagem desaparecerá em {duration / 1000} segundos...
                </p>
            </div>
        </div>
    );
};

export default FlashMessage;