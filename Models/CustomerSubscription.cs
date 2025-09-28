namespace SubscriptionDemo.Api.Models;

public class CustomerSubscription
{
    public int Id { get; set; }
    public string CustomerId { get; set; } = null!;
    public string CustomerName { get; set; } = null!;
    public string SubscriptionName { get; set; } = null!;
    public int SubscriptionCount { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
