using Microsoft.AspNetCore.Mvc;
using NotificationService.Models;
using NotificationService.Services;

namespace NotificationService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _service;

    public NotificationsController(INotificationService service)
    {
        _service = service;
    }

    /// <summary>Get all notifications</summary>
    [HttpGet]
    public ActionResult<IEnumerable<Notification>> GetAll()
        => Ok(_service.GetAll());

    /// <summary>Get notifications by recipient</summary>
    [HttpGet("recipient/{recipient}")]
    public ActionResult<IEnumerable<Notification>> GetByRecipient(string recipient)
        => Ok(_service.GetByRecipient(recipient));

    /// <summary>Send a notification</summary>
    [HttpPost]
    public ActionResult<Notification> Send([FromBody] SendNotificationDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Recipient))
            return BadRequest("Recipient is required.");
        if (string.IsNullOrWhiteSpace(dto.Message))
            return BadRequest("Message is required.");

        var notification = _service.Send(dto);
        return Created($"/api/notifications", notification);
    }

    /// <summary>Mark notification as read</summary>
    [HttpPatch("{id:guid}/read")]
    public IActionResult MarkAsRead(Guid id)
    {
        var result = _service.MarkAsRead(id);
        return result ? NoContent() : NotFound();
    }

    /// <summary>Health check</summary>
    [HttpGet("/health")]
    public IActionResult Health()
        => Ok(new { status = "healthy", service = "NotificationService" });
}
