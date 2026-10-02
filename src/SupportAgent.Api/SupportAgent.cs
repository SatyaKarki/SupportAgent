using Azure;
using Azure.AI.Projects;
using Azure.Identity;
using Microsoft.Agents.AI;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.AI;
using OpenAI;

namespace SupportAgent.Api;

public sealed class SupportAgent
{
    private readonly AIAgent _agent;

    public SupportAgent(SupportTools tools)
    {
        //var endpoint = Environment.GetEnvironmentVariable("AZURE_AI_PROJECT_ENDPOINT")
        //    ?? throw new InvalidOperationException("AZURE_AI_PROJECT_ENDPOINT is not set.");

        //var model = Environment.GetEnvironmentVariable("AZURE_AI_MODEL_DEPLOYMENT_NAME")
        //    ?? throw new InvalidOperationException("AZURE_AI_MODEL_DEPLOYMENT_NAME is not set.");
        var endpoint = "https://supportagentdemo.services.ai.azure.com/openai/v1";
        var model = "gpt-5.4-mini";

        var client = new AIProjectClient(new Uri(endpoint), new DefaultAzureCredential());


        var agentTools = new[]
        {
            AIFunctionFactory.Create(tools.GetOrderStatus),
            AIFunctionFactory.Create(tools.GetCustomerDetails),
            AIFunctionFactory.Create(tools.CreateSupportTicket)
        };

        _agent = client.AsAIAgent(
            model: model,
            name: "CustomerSupportAgent",
            instructions: """
                You are a customer support agent for a fictional e-commerce company.
                Use tools for current business data. Never invent order or customer data.
                Use CreateSupportTicket only when the user explicitly asks for a ticket.
                Report the generated ticket ID after successful creation.
                Keep responses concise.
                """,
            tools: agentTools);
    }

    public Task<AgentResponse> RunAsync(
        string message,
        CancellationToken cancellationToken = default)
        => _agent.RunAsync(message, cancellationToken: cancellationToken);
}
