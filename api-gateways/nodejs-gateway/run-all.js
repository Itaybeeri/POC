const { spawn } = require('child_process');
const path = require('path');

// Function to start a service
function startService(scriptPath, serviceName) {
    const service = spawn('node', [scriptPath], {
        stdio: 'inherit',
        shell: true
    });

    service.on('error', (error) => {
        console.error(`Error starting ${serviceName}:`, error);
    });

    return service;
}

// Start all services
console.log('Starting all services...');

// Start the API Gateway
const gateway = startService(
    path.join(__dirname, 'src', 'app.js'),
    'API Gateway'
);

// Start the User Service
const userService = startService(
    path.join(__dirname, 'src', 'mock-services', 'user-service.js'),
    'User Service'
);

// Start the Wallet Service
const walletService = startService(
    path.join(__dirname, 'src', 'mock-services', 'wallet-service.js'),
    'Wallet Service'
);

// Handle process termination
process.on('SIGINT', () => {
    console.log('\nShutting down all services...');
    gateway.kill();
    userService.kill();
    walletService.kill();
    process.exit();
}); 