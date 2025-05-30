const express = require('express');
const cors = require('cors');
const app = express();

// Middleware
app.use(cors());
app.use(express.json());

// Mock user data
const userProfile = {
    id: 1,
    name: 'John Doe',
    email: 'john@example.com'
};

// Health check endpoint
app.get('/ping', (req, res) => {
    res.send('pong');
});

// Login endpoint
app.post('/api/user/login', (req, res) => {
    res.json({
        message: 'login successful',
        token: 'mock-jwt-token'
    });
});

// Get user profile
app.get('/api/user/profile', (req, res) => {
    res.json(userProfile);
});

// Handle root path
app.get('/', (req, res) => {
    res.status(200).json({ message: 'User service is running' });
});

const PORT = process.env.PORT || 5202;
app.listen(PORT, () => {
    console.log(`Mock User Service is running on port ${PORT}`);
}); 