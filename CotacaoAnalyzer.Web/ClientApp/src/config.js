// src/config.js
const env = process.env.NODE_ENV || 'development';

const config = {
    development: {
        baseApiUrl: 'http://localhost:5000/api/v1' 
    },
    production: {
        baseApiUrl: `${window.location.origin}/api/v1`
    }
};

export default config[env];