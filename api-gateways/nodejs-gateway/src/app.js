const express = require('express');
const cors = require('cors');
const helmet = require('helmet');
const morgan = require('morgan');
const rateLimit = require('express-rate-limit');
const { createProxyMiddleware } = require('http-proxy-middleware');
require('dotenv').config();

const app = express();

// Middleware
app.use(helmet()); // Security headers
app.use(cors()); // Enable CORS
app.use(express.json()); // Parse JSON bodies
app.use(morgan('dev')); // Logging

// Rate limiting
const limiter = rateLimit({
    windowMs: process.env.RATE_LIMIT_WINDOW_MS || 900000, // 15 minutes
    max: process.env.RATE_LIMIT_MAX_REQUESTS || 100 // limit each IP to 100 requests per windowMs
});
app.use(limiter);

// Proxy configuration
const walletServiceProxy = createProxyMiddleware({
    target: process.env.WALLET_SERVICE_URL || 'http://localhost:5201',
    changeOrigin: true,
    pathRewrite: {
        '^/api/wallet': '/api/wallet'  // Keep the /api/wallet prefix as it's handled by the wallet service
    }
});

const userServiceProxy = createProxyMiddleware({
    target: process.env.USER_SERVICE_URL || 'http://localhost:5202',
    changeOrigin: true,
    pathRewrite: {
        '^/api/user': '/api/user'  // Keep the /api/user prefix as it's handled by the user service
    }
});

// Routes
app.use('/api/wallet', walletServiceProxy);
app.use('/api/user', userServiceProxy);

// Health check endpoint (matching Golang's /ping endpoint)
app.get('/ping', (req, res) => {
    res.status(200).send('pong');
});

// Error handling middleware
app.use((err, req, res, next) => {
    console.error(err.stack);
    res.status(500).json({
        error: 'Internal Server Error',
        message: process.env.NODE_ENV === 'development' ? err.message : undefined
    });
});

const PORT = process.env.PORT || 8080; // Matching Golang's default port
app.listen(PORT, () => {
    console.log(`API Gateway is running on port ${PORT}`);
}); 