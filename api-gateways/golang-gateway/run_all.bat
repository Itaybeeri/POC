@echo off
echo Starting Wallet Service...
start cmd /k "go run wallet\wallet.go"

echo Starting User Service...
start cmd /k "go run user\user.go"

echo Starting API Gateway...
start cmd /k "go run gateway.go"

echo All services started in separate windows.
