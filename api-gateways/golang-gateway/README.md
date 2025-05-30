This POC if for createing an API gateway using the go language and echo web framework
The Gateway will run on port 8080 defined in the config file
It acts as a reverse proxy (means hiding the service inside the organization)
The code has centralized logging using echo middleware
and a build in healthcheck (/ping)

Launch the code as is will listen on port 8080
Get automatic routing of HTTP requests to two backend services:
Requests to /api/wallet/_ will be forwarded to http://localhost:5201
Requests to /api/user/_ will be forwarded to http://localhost:5202

Have a health check endpoint at /ping returning a simple "pong" message.

Get logging of all HTTP requests and recovery from unexpected crashes via built-in middleware.

Before running this project please run the init script
On Linux - sh init_scripts/install_echo_deps.sh
On windows - .\init_scripts\install_echo_deps.ps1

To run this code simplewith the mock services just run
On Linux -
chmod +x run_all.sh
./run_all.sh

On windows - run_all.bat

Use your browser for the routing examples

(GET) http://localhost:8080/ping
(GET) http://localhost:8080/api/wallet/balance
(POST) http://localhost:8080/api/wallet/charge
(POST) http://localhost:8080/api/user/login
(GET) http://localhost:8080/api/user/profile
