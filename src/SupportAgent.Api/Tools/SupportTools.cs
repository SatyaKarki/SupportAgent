using System.ComponentModel;

namespace SupportAgent.Api;

public sealed class SupportTools
{
    private readonly DemoDataStore _store;
    private readonly ILogger<SupportTools> _logger;

    public SupportTools(DemoDataStore store, ILogger<SupportTools> logger)
    {
        _store = store;
        _logger = logger;
    }

    [Description("Gets the current status and delivery information for an order.")]
    public object GetOrderStatus(
        [Description("Order ID, for example ORD-1005.")] string orderId)
    {
        _logger.LogInformation("Tool called: GetOrderStatus({OrderId})", orderId);

        if (!_store.TryGetOrder(orderId, out var order))
            return new { found = false, message = $"Order {orderId} was not found." };

        return new
        {
            found = true,
            order.OrderId,
            order.CustomerId,
            order.Status,
            order.FulfillmentStatus,
            order.ExpectedDelivery,
            order.DelayReason
        };
    }

    [Description("Gets customer profile and support history.")]
    public object GetCustomerDetails(
        [Description("Customer ID, for example CUST-1001.")] string customerId)
    {
        _logger.LogInformation("Tool called: GetCustomerDetails({CustomerId})", customerId);

        if (!_store.TryGetCustomer(customerId, out var customer))
            return new { found = false, message = $"Customer {customerId} was not found." };

        return new
        {
            found = true,
            customer.CustomerId,
            customer.Name,
            customer.Email,
            customer.Tier,
            customer.SupportHistory
        };
    }

    [Description("Creates a support ticket. Only call when explicitly requested by the user.")]
    public object CreateSupportTicket(
        [Description("Customer ID.")] string customerId,
        [Description("Order ID.")] string orderId,
        [Description("Short ticket reason.")] string reason)
    {
        _logger.LogInformation(
            "Tool called: CreateSupportTicket({CustomerId}, {OrderId})",
            customerId, orderId);

        var ticketId = _store.AddTicket(customerId, orderId, reason);

        return new
        {
            created = true,
            ticketId,
            customerId,
            orderId,
            reason
        };
    }
}
