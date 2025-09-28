using System.ComponentModel.DataAnnotations;

namespace SubscriptionDemo.Api.DTOs;

public class SubscriptionQueryDto
{
    [Required] public string CustomerId { get; set; } = null!;
    public string? SubscriptionName { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class SubscriptionCreateDto
{
    [Required] public string CustomerId { get; set; } = null!;
    [Required] public string CustomerName { get; set; } = null!;
    [Required] public string SubscriptionName { get; set; } = null!;
    public int SubscriptionCount { get; set; } = 0;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; } = true;
}

public class UpdateCountDto
{
    [Required] public string CustomerId { get; set; } = null!;
    [Required] public string SubscriptionName { get; set; } = null!;
    [Required] public int Delta { get; set; }
}
