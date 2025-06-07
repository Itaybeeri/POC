# Technology POC Repository

This repository contains various Proof of Concept (POC) implementations in different programming languages and frameworks. Each POC demonstrates how to implement the same functionality using different technology stacks, allowing for comparison of approaches, performance, and developer experience.

## Current POCs

### 1. API Gateway Implementation

Located in `api-gateways/`

A microservices API Gateway implementation comparing different approaches:

#### Golang Implementation

- Built with Go and Echo framework
- Features:
  - Lightweight and high-performance
  - Built-in concurrency support
  - Strong typing and compile-time checks
  - Minimal memory footprint
  - Native JSON handling

#### Node.js Implementation

- Built with Express.js
- Features:
  - Event-driven architecture
  - Rich ecosystem of middleware
  - Easy JSON handling
  - Asynchronous I/O
  - Extensive npm package support

#### .NET 8 Implementation

- Built with .NET 8 and YARP (Yet Another Reverse Proxy)
- Features:
  - High performance and scalability
  - Modern minimal API style
  - YARP for reverse proxying
  - Strong typing and tooling
  - Swagger/OpenAPI documentation

### 2. Logging Extension Method

Located in `.net/log extenstion method/`

A dual-logging system implementation that separates application logs from infrastructure logs:

- Features:
  - Two separate logging systems (AppsLogger and InfraLogger)
  - Elasticsearch integration for log storage
  - Configurable through appsettings.json
  - Enable/disable logging systems independently
  - REST API endpoints for logging
  - Support for multiple log levels
  - Additional data support in logs

### 3. MCP Server Demo

Located in `.net/McpDemoServer/`

A Model Context Protocol (MCP) server implementation that demonstrates how to expose backend services and data for AI models:

- Features:
  - Standardized interface for AI model integration
  - Customer service API with in-memory data store
  - Swagger/OpenAPI documentation
  - Modern .NET 8 Web API implementation
  - RESTful endpoints for customer data access
  - Extensible architecture for adding more services

For detailed information about each POC, including setup instructions and API documentation, please refer to the README.md in the respective subdirectories.

## Project Structure

```
.
├── api-gateways/           # API Gateway POC implementations
│   ├── golang-gateway/    # Go implementation
│   ├── nodejs-gateway/    # Node.js implementation
│   └── dotnet-gateway/    # .NET 8 implementation
├── .net/                  # .NET related POCs
│   ├── log extenstion method/  # Dual-logging system implementation
│   └── McpDemoServer/     # Model Context Protocol server
└── README.md              # This file
```

## Purpose

This repository serves as a collection of technology comparisons, helping developers:

- Compare different approaches to solving the same problem
- Understand the strengths and weaknesses of various technology stacks
- Make informed decisions about technology choices
- Learn from real-world implementation examples

## Contributing

We welcome contributions of new POCs or improvements to existing ones. Please follow these steps:

1. Fork the repository
2. Create a new branch for your POC
3. Add your implementation in a new directory
4. Include a comprehensive README.md with:
   - Overview of the POC
   - Implementation details
   - Setup instructions
   - Technology comparisons
   - Performance metrics (if applicable)
5. Update this main README.md with a brief description of your POC
6. Submit a pull request

## Guidelines for New POCs

When adding a new POC, please ensure:

1. **Clear Purpose**: Define what problem the POC solves
2. **Multiple Implementations**: Include at least two different technology approaches
3. **Documentation**: Provide comprehensive setup and usage instructions
4. **Comparisons**: Include pros and cons of each implementation
5. **Code Quality**: Follow best practices for each technology
6. **Testing**: Include basic tests and performance benchmarks
7. **Dependencies**: Clearly list all requirements and dependencies

## License

This project is licensed under the MIT License - see the LICENSE file for details.
