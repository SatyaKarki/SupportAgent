dotnet build SupportAgent.sln
dotnet run --project src/SupportAgent.Api
# SupportAgent — Complete Solution

`SupportAgent.Api` is a .NET 10 ASP.NET Core web project that demonstrates using the Microsoft Agents SDK, Microsoft Foundry, and Azure AI Projects to implement a customer support agent.

## Contents

- `src/SupportAgent.Api` — ASP.NET Core Web API and agent implementation

## Quick start

1. Clone the repository and open the solution `SupportAgent.sln` in Visual Studio or VS Code.
2. Set required environment variables (see below).
3. Build and run:

dotnet build SupportAgent.sln
dotnet run --project src/SupportAgent.Api
```bash
dotnet restore SupportAgent.sln
dotnet build SupportAgent.sln
dotnet run --project src/SupportAgent.Api
```

## Requirements

- .NET 10 SDK
- An Azure AI Projects endpoint or compatible service

## Environment variables

Required:

- `AZURE_AI_PROJECT_ENDPOINT` — URL of your Azure AI Projects endpoint (example: `https://<resource>.ai.azure.com/api/projects/<project>`)
- `AZURE_AI_MODEL_DEPLOYMENT_NAME` — The deployed model name/deployment id to use

Example (PowerShell):

```powershell
$env:AZURE_AI_PROJECT_ENDPOINT = "https://your-endpoint"
$env:AZURE_AI_MODEL_DEPLOYMENT_NAME = "your-deployment"
```

## Key packages and notes

The project was adjusted to resolve transitive dependency and type conflicts. Notable package decisions in the project file `src/SupportAgent.Api/SupportAgent.Api.csproj`:

- `Azure.AI.Projects` pinned to `2.1.0-beta.4` (to satisfy `Microsoft.Agents.AI.Foundry` transitive dependency)
- `Microsoft.Extensions.AI` pinned to `10.6.0`
- `Azure.Identity` is not referenced directly to avoid a duplicate `DefaultAzureCredential` type conflict. The project relies on transitive dependencies to provide credential types.

If you want an explicit `Azure.Identity` reference, ensure version alignment with `Azure.Core` and other Azure libraries to avoid compile-time type conflicts.

## How the agent is wired

The `SupportAgent` creates an `AIProjectClient` and converts it to an `AIAgent` using `AsAIAgent(...)`. Tool functions included in the sample:

- `GetOrderStatus`
- `GetCustomerDetails`
- `CreateSupportTicket`

Example snippet from the project:

```csharp
var client = new Azure.AI.Projects.AIProjectClient(new Uri(endpoint), new Azure.Identity.DefaultAzureCredential());

_agent = client.AsAIAgent(
    model: model,
    name: "CustomerSupportAgent",
    instructions: "...",
    tools: agentTools);
```

## Demo prompts

Try prompts such as:

- `What is the status of order ORD-1005?`
- `Order ORD-1005 is delayed. Who is the customer and what should support recommend?`
- `Create a support ticket for customer CUST-1001 because order ORD-1005 is delayed.`

## Troubleshooting

- Duplicate `DefaultAzureCredential` type (CS0433):
  - Symptom: compile error indicating `DefaultAzureCredential` exists in both `Azure.Core` and `Azure.Identity`.
  - Fix applied in this repo: remove the direct `Azure.Identity` `PackageReference` and rely on transitive dependencies. Alternatively, align package versions if you require explicit `Azure.Identity`.

- Package downgrade warnings (NU1605):
  - Fix: pin packages to versions compatible with transitive requirements (see "Key packages and notes").

## Contributing

- Keep changes small and package updates aligned with transitive dependency constraints.
- Update this README when adding new environment variables or runtime requirements.

## License

MIT License