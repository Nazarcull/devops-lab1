using FluentAssertions;
using NotificationService.Models;
using NotificationService.Services;
using Xunit;

namespace NotificationService.Tests;

public class InMemoryNotificationServiceTests
{
    private readonly InMemoryNotificationService _service = new();

    [Fact]
    public void Send_ValidDto_ReturnsNotificationWithId()
    {
        // Arrange
        var dto = new SendNotificationDto
        {
            Recipient = "user@example.com",
            Message = "Hello!",
            Type = NotificationType.Info
        };

        // Act
        var result = _service.Send(dto);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBe(Guid.Empty);
        result.Recipient.Should().Be("user@example.com");
        result.Message.Should().Be("Hello!");
        result.IsRead.Should().BeFalse();
    }

    [Fact]
    public void GetAll_ReturnsAllSentNotifications()
    {
        // Arrange
        _service.Send(new SendNotificationDto { Recipient = "a@a.com", Message = "Msg 1" });
        _service.Send(new SendNotificationDto { Recipient = "b@b.com", Message = "Msg 2" });

        // Act
        var result = _service.GetAll();

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public void GetByRecipient_FiltersCorrectly()
    {
        // Arrange
        _service.Send(new SendNotificationDto { Recipient = "alice@example.com", Message = "For Alice" });
        _service.Send(new SendNotificationDto { Recipient = "bob@example.com", Message = "For Bob" });
        _service.Send(new SendNotificationDto { Recipient = "alice@example.com", Message = "Also for Alice" });

        // Act
        var result = _service.GetByRecipient("alice@example.com").ToList();

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(n => n.Recipient == "alice@example.com");
    }

    [Fact]
    public void MarkAsRead_ExistingId_ReturnsTrue()
    {
        // Arrange
        var notification = _service.Send(new SendNotificationDto
        {
            Recipient = "user@example.com",
            Message = "Read me"
        });

        // Act
        var result = _service.MarkAsRead(notification.Id);
        var updated = _service.GetAll().First(n => n.Id == notification.Id);

        // Assert
        result.Should().BeTrue();
        updated.IsRead.Should().BeTrue();
    }

    [Fact]
    public void MarkAsRead_NonExistingId_ReturnsFalse()
    {
        // Act
        var result = _service.MarkAsRead(Guid.NewGuid());

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Send_MultipleNotifications_OrderedByCreatedAtDescending()
    {
        // Arrange
        _service.Send(new SendNotificationDto { Recipient = "x@x.com", Message = "First" });
        Thread.Sleep(5); // ensure different timestamps
        _service.Send(new SendNotificationDto { Recipient = "x@x.com", Message = "Second" });

        // Act
        var result = _service.GetAll().ToList();

        // Assert
        result[0].Message.Should().Be("Second");
        result[1].Message.Should().Be("First");
    }
}
