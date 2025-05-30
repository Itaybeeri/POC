# Start the Gateway
Start-Process -NoNewWindow -FilePath "dotnet" -ArgumentList "run --project src/Gateway/Gateway.csproj"

# Start the User Service
Start-Process -NoNewWindow -FilePath "dotnet" -ArgumentList "run --project src/UserService/UserService.csproj"

# Start the Wallet Service
Start-Process -NoNewWindow -FilePath "dotnet" -ArgumentList "run --project src/WalletService/WalletService.csproj"

Write-Host "All services are running. Press Ctrl+C to stop all services." 