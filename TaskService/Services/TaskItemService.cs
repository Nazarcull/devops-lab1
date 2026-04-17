using Microsoft.EntityFrameworkCore;
using TaskService.Data;
using TaskService.Models;

namespace TaskService.Services;

public interface ITaskService
{
    Task<IEnumerable<TaskItem>> GetAllAsync();
    Task<TaskItem?> GetByIdAsync(int id);
    Task<TaskItem> CreateAsync(CreateTaskDto dto);
    Task<TaskItem?> UpdateAsync(int id, UpdateTaskDto dto);
    Task<bool> DeleteAsync(int id);
}

public class TaskItemService : ITaskService
{
    private readonly AppDbContext _db;

    public TaskItemService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync()
        => await _db.Tasks.OrderByDescending(t => t.CreatedAt).ToListAsync();

    public async Task<TaskItem?> GetByIdAsync(int id)
        => await _db.Tasks.FindAsync(id);

    public async Task<TaskItem> CreateAsync(CreateTaskDto dto)
    {
        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            Priority = dto.Priority,
            CreatedAt = DateTime.UtcNow
        };
        _db.Tasks.Add(task);
        await _db.SaveChangesAsync();
        return task;
    }

    public async Task<TaskItem?> UpdateAsync(int id, UpdateTaskDto dto)
    {
        var task = await _db.Tasks.FindAsync(id);
        if (task is null) return null;

        if (dto.Title is not null) task.Title = dto.Title;
        if (dto.Description is not null) task.Description = dto.Description;
        if (dto.Priority is not null) task.Priority = dto.Priority.Value;
        if (dto.IsCompleted is not null)
        {
            task.IsCompleted = dto.IsCompleted.Value;
            task.CompletedAt = dto.IsCompleted.Value ? DateTime.UtcNow : null;
        }

        await _db.SaveChangesAsync();
        return task;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var task = await _db.Tasks.FindAsync(id);
        if (task is null) return false;

        _db.Tasks.Remove(task);
        await _db.SaveChangesAsync();
        return true;
    }
}
