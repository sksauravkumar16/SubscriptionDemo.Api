using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubscriptionDemo.Api.DTOs;
using SubscriptionDemo.Api.Models;
using SubscriptionDemo.Api.Repositories;

namespace SubscriptionDemo.Api.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CustomerSubscriptionController : ControllerBase
{
    private readonly ISubscriptionRepository _repo;
    public CustomerSubscriptionController(ISubscriptionRepository repo) => _repo = repo;

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] SubscriptionQueryDto query)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var items = await _repo.GetAsync(query.CustomerId, query.SubscriptionName, query.StartDate, query.EndDate);
        return Ok(items);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] SubscriptionCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var model = new CustomerSubscription
        {
            CustomerId = dto.CustomerId,
            CustomerName = dto.CustomerName,
            SubscriptionName = dto.SubscriptionName,
            SubscriptionCount = dto.SubscriptionCount,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            IsActive = dto.IsActive
        };
        var id = await _repo.CreateAsync(model);
        return CreatedAtAction(nameof(Get), new { id }, new { id });
    }

    [HttpPost("update-count")]
    public async Task<IActionResult> UpdateCount([FromBody] UpdateCountDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var ok = await _repo.UpdateCountAsync(dto.CustomerId, dto.SubscriptionName, dto.Delta);
        if (!ok) return NotFound(new { message = "Subscription not found" });
        return Ok(new { message = "Updated" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _repo.DeleteAsync(id);
        if (!ok) return NotFound(new { message = "Not found" });
        return NoContent();
    }
}
