using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TaskService.Data;
using TaskService.Models;
using TaskService.Services;
using Xunit;

namespace TaskService.Tests;

public class TaskItemServiceTests : IDisposable
{
    private readonly AppDbContext _db;
    private readonly TaskItemService _service;

    public TaskItemServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _db = new AppDbContext(options);
        _service = new TaskItemService(_db);
    }

    [Fact]
    public async Task CreateAsync_ValidDto_ReturnsCreatedTask()
    {
        // Arrange
        var dto = new CreateTaskDto
        {
            Title = "Test Task",
            Description = "Test Description",
            Priority = TaskPriority.High
        };

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.Title.Should().Be("Test Task");
        result.Priority.Should().Be(TaskPriority.High);
        result.IsCompleted.Should().BeFalse();
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllTasks()
    {
        // Arrange
        await _service.CreateAsync(new CreateTaskDto { Title = "Task 1" });
        await _service.CreateAsync(new CreateTaskDto { Title = "Task 2" });
        await _service.CreateAsync(new CreateTaskDto { Title = "Task 3" });

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsTask()
    {
        // Arrange
        var created = await _service.CreateAsync(new CreateTaskDto { Title = "Find Me" });

        // Act
        var result = await _service.GetByIdAsync(created.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Title.Should().Be("Find Me");
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        // Act
        var result = await _service.GetByIdAsync(9999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_MarkCompleted_SetsCompletedAt()
    {
        // Arrange
        var created = await _service.CreateAsync(new CreateTaskDto { Title = "To Complete" });

        // Act
        var result = await _service.UpdateAsync(created.Id, new UpdateTaskDto { IsCompleted = true });

        // Assert
        result.Should().NotBeNull();
        result!.IsCompleted.Should().BeTrue();
        result.CompletedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_NonExistingId_ReturnsNull()
    {
        // Act
        var result = await _service.UpdateAsync(9999, new UpdateTaskDto { Title = "Ghost" });

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_ExistingId_ReturnsTrueAndRemovesTask()
    {
        // Arrange
        var created = await _service.CreateAsync(new CreateTaskDto { Title = "Delete Me" });

        // Act
        var deleted = await _service.DeleteAsync(created.Id);
        var found = await _service.GetByIdAsync(created.Id);

        // Assert
        deleted.Should().BeTrue();
        found.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_NonExistingId_ReturnsFalse()
    {
        // Act
        var result = await _service.DeleteAsync(9999);

        // Assert
        result.Should().BeFalse();
    }

    public void Dispose() => _db.Dispose();
}
