namespace SupportAgent.Api;

public sealed class DemoDataStore
{
    private readonly Dictionary<string, Order> _orders = new(StringComparer.OrdinalIgnoreCase)
    {
        ["ORD-1005"] = new(
            "ORD-1005", "CUST-1001", "Delayed", "Shipped",
            DateTimeOffset.UtcNow.AddDays(2),
            "Carrier reported a weather-related delay.")
    };

    private readonly Dictionary<string, Customer> _customers = new(StringComparer.OrdinalIgnoreCase)
    {
        ["CUST-1001"] = new(
            "CUST-1001", "Alice Johnson", "alice@example.com",
            "Premium", "Customer has contacted support twice this month.")
    };

    private readonly List<SupportTicket> _tickets = new();

    public bool TryGetOrder(string id, out Order? order) => _orders.TryGetValue(id, out order);
    public bool TryGetCustomer(string id, out Customer? customer) => _customers.TryGetValue(id, out customer);

    public string AddTicket(string customerId, string orderId, string reason)
    {
        var id = $"TKT-{1000 + _tickets.Count + 1}";
        _tickets.Add(new SupportTicket(id, customerId, orderId, reason, DateTimeOffset.UtcNow));
        return id;
    }
}

public sealed record Order(
    string OrderId, string CustomerId, string Status,
    string FulfillmentStatus, DateTimeOffset ExpectedDelivery, string DelayReason);

public sealed record Customer(
    string CustomerId, string Name, string Email,
    string Tier, string SupportHistory);

public sealed record SupportTicket(
    string TicketId, string CustomerId, string OrderId,
    string Reason, DateTimeOffset CreatedAt);
