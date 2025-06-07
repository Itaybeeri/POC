using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

class Program
{
    private static readonly HttpClient _httpClient = new HttpClient();
    private static readonly string _serverBaseUrl = "http://localhost:5407";
    private static readonly string _anthropicApiUrl = "https://api.anthropic.com/v1/messages";

    static async Task Main(string[] args)
    {
        Console.WriteLine("MCP Client - AI-Powered Customer Data Query");
        Console.WriteLine("Type 'exit' to quit");
        Console.WriteLine();

        while (true)
        {
            Console.Write("Enter your query: ");
            var query = Console.ReadLine();

            if (string.IsNullOrEmpty(query) || query.ToLower() == "exit")
                break;

            try
            {
                var result = await ProcessQueryAsync(query);
                Console.WriteLine("\nResult:");
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Error: {ex.InnerException.Message}");
                }
            }

            Console.WriteLine();
        }
    }

    static async Task<string> ProcessQueryAsync(string query)
    {
        try
        {
            // First, get the MCP metadata to understand available tools
            Console.WriteLine("Fetching MCP metadata...");
            var metadata = await GetMcpMetadataAsync();
            Console.WriteLine($"Metadata received: {JsonSerializer.Serialize(metadata)}");

            // Use Claude to understand the query and determine which tool to use
            var prompt = $@"You are an AI assistant that helps users query customer data. \nAvailable tools: {JsonSerializer.Serialize(metadata.tools)}\nUser query: {query}\nDetermine which tool to use and provide the parameters in JSON format. \nOnly respond with the JSON, no other text.";

            Console.WriteLine("Sending prompt to Claude...");
            var toolCall = await GetToolCallFromClaudeAsync(query, metadata);
            Console.WriteLine($"Tool call received: {JsonSerializer.Serialize(toolCall)}");

            // Execute the appropriate tool
            Console.WriteLine($"Executing tool: {toolCall.Name}");
            return toolCall.Name switch
            {
                "getCustomer" => await GetCustomerAsync(toolCall.Parameters.id),
                "listCustomers" => await ListCustomersAsync(),
                _ => throw new Exception($"Unknown tool: {toolCall.Name}")
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in ProcessQueryAsync: {ex.Message}");
            throw;
        }
    }

    private static async Task<ToolCall> GetToolCallFromClaudeAsync(string query, McpMetadata metadata)
    {
        try
        {
            // For testing, return a mock response based on the query
            Console.WriteLine("Using mock response for testing");

            // Check if the query is asking for all customers
            var lowerQuery = query.ToLower();
            if (lowerQuery.Contains("all") || lowerQuery.Contains("list"))
            {
                return new ToolCall
                {
                    Name = "listCustomers",
                    Parameters = new ToolParameters
                    {
                        type = "object",
                        properties = new Dictionary<string, ParameterProperty>(),
                        required = Array.Empty<string>()
                    }
                };
            }

            // Extract customer ID from the query if present (e.g., 'get details for 2', 'customer 2')
            var match = System.Text.RegularExpressions.Regex.Match(lowerQuery, @"(?:customer|for)\s*(\d+)");
            string customerId = match.Success ? match.Groups[1].Value : "1";

            return new ToolCall
            {
                Name = "getCustomer",
                Parameters = new ToolParameters
                {
                    type = "object",
                    properties = new Dictionary<string, ParameterProperty>(),
                    required = Array.Empty<string>(),
                    id = customerId
                }
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting tool call from Claude: {ex.Message}");
            throw;
        }
    }

    static async Task<McpMetadata> GetMcpMetadataAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_serverBaseUrl}/mcp/metadata");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<McpMetadata>(content);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetMcpMetadataAsync: {ex.Message}");
            throw;
        }
    }

    static async Task<string> GetCustomerAsync(string id)
    {
        try
        {
            Console.WriteLine($"Getting customer with ID: {id}");
            var response = await _httpClient.GetAsync($"{_serverBaseUrl}/mcp/customer/{id}");
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Server response: {content}");
            response.EnsureSuccessStatusCode();
            return content;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetCustomerAsync: {ex.Message}");
            throw;
        }
    }

    static async Task<string> ListCustomersAsync()
    {
        try
        {
            Console.WriteLine("Listing all customers");
            var response = await _httpClient.GetAsync($"{_serverBaseUrl}/mcp/customers");
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Server response: {content}");
            response.EnsureSuccessStatusCode();
            return content;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in ListCustomersAsync: {ex.Message}");
            throw;
        }
    }
}

public class McpMetadata
{
    public string version { get; set; }
    public Tool[] tools { get; set; }
    public Resource[] resources { get; set; }
}

public class Tool
{
    public string name { get; set; }
    public string description { get; set; }
    public ToolParameters parameters { get; set; }
}

public class ToolParameters
{
    public string type { get; set; }
    public Dictionary<string, ParameterProperty> properties { get; set; }
    public string[] required { get; set; }
    public string id { get; set; }
}

public class ParameterProperty
{
    public string type { get; set; }
    public string description { get; set; }
}

public class Resource
{
    public string name { get; set; }
    public string description { get; set; }
    public Dictionary<string, ParameterProperty> properties { get; set; }
}

public class ToolCall
{
    public string Name { get; set; }
    public ToolParameters Parameters { get; set; }
}
