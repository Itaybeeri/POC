const express = require('express');
const cors = require('cors');
const app = express();

// Middleware
app.use(cors());
app.use(express.json());

// Mock database
let wallets = [
    { id: 1, userId: 1, balance: 1000, currency: 'USD' },
    { id: 2, userId: 2, balance: 2000, currency: 'USD' }
];

// Get all wallets
app.get('/', (req, res) => {
    res.json(wallets);
});

// Get wallet by ID
app.get('/:id', (req, res) => {
    const wallet = wallets.find(w => w.id === parseInt(req.params.id));
    if (!wallet) return res.status(404).json({ error: 'Wallet not found' });
    res.json(wallet);
});

// Get wallet by user ID
app.get('/user/:userId', (req, res) => {
    const wallet = wallets.find(w => w.userId === parseInt(req.params.userId));
    if (!wallet) return res.status(404).json({ error: 'Wallet not found for this user' });
    res.json(wallet);
});

// Create new wallet
app.post('/', (req, res) => {
    const newWallet = {
        id: wallets.length + 1,
        userId: req.body.userId,
        balance: req.body.balance || 0,
        currency: req.body.currency || 'USD'
    };
    wallets.push(newWallet);
    res.status(201).json(newWallet);
});

// Update wallet balance
app.put('/:id/balance', (req, res) => {
    const wallet = wallets.find(w => w.id === parseInt(req.params.id));
    if (!wallet) return res.status(404).json({ error: 'Wallet not found' });

    const amount = parseFloat(req.body.amount);
    if (isNaN(amount)) {
        return res.status(400).json({ error: 'Invalid amount' });
    }

    wallet.balance += amount;
    res.json(wallet);
});

// Transfer between wallets
app.post('/transfer', (req, res) => {
    const { fromWalletId, toWalletId, amount } = req.body;
    
    const fromWallet = wallets.find(w => w.id === parseInt(fromWalletId));
    const toWallet = wallets.find(w => w.id === parseInt(toWalletId));

    if (!fromWallet || !toWallet) {
        return res.status(404).json({ error: 'One or both wallets not found' });
    }

    const transferAmount = parseFloat(amount);
    if (isNaN(transferAmount) || transferAmount <= 0) {
        return res.status(400).json({ error: 'Invalid transfer amount' });
    }

    if (fromWallet.balance < transferAmount) {
        return res.status(400).json({ error: 'Insufficient funds' });
    }

    fromWallet.balance -= transferAmount;
    toWallet.balance += transferAmount;

    res.json({
        message: 'Transfer successful',
        fromWallet,
        toWallet
    });
});

// Health check
app.get('/ping', (req, res) => {
    res.send('pong');
});

// Get wallet balance
app.get('/api/wallet/balance', (req, res) => {
    res.json({
        balance: 150.75,
        currency: 'USD'
    });
});

// Charge wallet
app.post('/api/wallet/charge', (req, res) => {
    res.json({
        status: 'charged'
    });
});

// Handle root path
app.get('/', (req, res) => {
    res.status(200).json({ message: 'Wallet service is running' });
});

const PORT = process.env.PORT || 5201;
app.listen(PORT, () => {
    console.log(`Mock Wallet Service is running on port ${PORT}`);
}); 