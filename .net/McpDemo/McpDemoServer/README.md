# MCP Server Demo (.NET 8)

This project is a minimal Model Context Protocol (MCP) server built with ASP.NET Core and C#. It demonstrates how to expose backend services and data in a standardized way for use by AI models, chatbots, or other clients.

## What is MCP (Model Context Protocol)?

**MCP (Model Context Protocol)** is a standard protocol for exposing backend resources and tools to AI models and clients. It defines a common way for services to describe and provide access to their data and functions, typically using JSON over HTTP or other transports. MCP enables seamless integration between AI models and backend systems, reducing the need for custom connectors or integrations for each new service or model.

### What does MCP provide?

- **Standardization:** A unified interface for accessing data and tools, making it easier for AI models and clients to interact with various backends.
- **Interoperability:** Any MCP-compliant client (such as an AI model, chatbot, or automation tool) can interact with any MCP-compliant server, regardless of the underlying technology.
- **Scalability:** As you add more services or models, MCP helps avoid the "N×M" integration problem by providing a single protocol for all interactions.
- **Rapid Integration:** New tools and resources can be exposed to AI models quickly, without writing custom adapters for each use case.

## How AI Models and Clients Should Use This MCP Server

AI models, chatbots, or other MCP-compatible clients can use this server to access customer data and business logic in real time. By making HTTP requests to the provided endpoints, an AI system can:

- Retrieve a list of all customers for context-aware responses or recommendations.
- Fetch details about a specific customer to personalize interactions or perform lookups.
- Integrate business data into conversations, automations, or workflows without custom backend integration.

### Example Use Cases for AI Integration

- **Conversational AI:** A chatbot can answer questions like "Who is customer 1?" by calling the `/mcp/customer/1` endpoint and returning the result to the user.
- **Automated Agents:** An AI agent can fetch all customers and process them for reporting, analytics, or batch operations.
- **Personalized Recommendations:** AI models can use customer data to tailor suggestions or actions based on real-time backend information.

### What This Model Provides to AI Systems

- **Live Data Access:** AI models can access up-to-date customer information directly from your backend.
- **Tool Invocation:** The endpoints act as callable tools that AI can use to perform specific actions or queries.
- **Extensible API:** You can add more endpoints or business logic, and AI clients will be able to use them immediately via the standardized MCP interface.

## How AI Models Discover Available Endpoints

AI models or clients can discover the available endpoints in several ways:

- **Swagger/OpenAPI Documentation:** This project includes Swagger UI, which provides interactive API documentation. You can access it at `/swagger` when running the server in development mode. The OpenAPI specification (available at `/swagger/v1/swagger.json`) can be parsed by AI systems to understand the available endpoints, their parameters, and response formats.
- **MCP Metadata:** Some MCP implementations include a metadata endpoint (e.g., `/mcp/metadata`) that returns a JSON description of all available tools and resources. AI clients can query this endpoint to dynamically discover what actions they can perform.
- **Static Configuration:** In some cases, the AI system is pre-configured with the list of available endpoints and their usage, especially if the server's API is stable and well-documented.

By using these discovery mechanisms, AI models can dynamically adapt to changes in the server's API and ensure they are using the correct endpoints for their tasks.

## Features

- **Customer Service API:**
  - Provides endpoints to list all customers and fetch a customer by ID.
  - Uses an in-memory data store for demonstration purposes.
- **MCP Pattern:**
  - Exposes backend resources and tools in a way that is easy for AI models and clients to consume, typically using JSON over HTTP.
- **Modern .NET 8 Web API:**
  - Built with .NET 8 and follows best practices for dependency injection and minimal API design.
- **API Documentation:**
  - Includes Swagger/OpenAPI documentation for easy API discovery and testing.

## Endpoints

- `GET /mcp/customers` — Returns a list of all customers as JSON.
- `GET /mcp/customer/{id}` — Returns details for a specific customer by ID.

## How to Run

1. **Restore dependencies:**
   ```sh
   dotnet restore
   ```
2. **Run the server:**

   ```sh
   dotnet run --project MpcDemoServer
   ```

   By default, the server listens on `http://localhost:5047`.

3. **Access Swagger UI:**
   Open your browser and navigate to `http://localhost:5047/swagger` to view the interactive API documentation.

## How to Use

- List all customers:
  ```sh
  curl http://localhost:5047/mcp/customers
  ```
- Get a specific customer (e.g., ID 1):
  ```sh
  curl http://localhost:5047/mcp/customer/1
  ```

## Extensibility

- You can add more endpoints, connect to a real database, or implement authentication as needed.
- The project is designed to be a starting point for exposing your own data and services to AI models or other MCP-compatible clients.

## Why Use This?

- **Standardization:** Easily expose your backend to any AI model or client that understands MCP.
- **Rapid Prototyping:** Quickly make your data and services available for experimentation or production use.
- **Extensible:** Add more business logic, resources, or integrations as your needs grow.

---

Feel free to use and extend this project for your own MCP server implementations!
