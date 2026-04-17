using Microsoft.AspNetCore.Mvc;
using TaskService.Models;
using TaskService.Services;

namespace TaskService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    /// <summary>Get all tasks</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetAll()
        => Ok(await _taskService.GetAllAsync());

    /// <summary>Get task by ID</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskItem>> GetById(int id)
    {
        var task = await _taskService.GetByIdAsync(id);
        return task is null ? NotFound() : Ok(task);
    }

    /// <summary>Create a new task</summary>
    [HttpPost]
    public async Task<ActionResult<TaskItem>> Create([FromBody] CreateTaskDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
            return BadRequest("Title is required.");

        var task = await _taskService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }

    /// <summary>Update an existing task</summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<TaskItem>> Update(int id, [FromBody] UpdateTaskDto dto)
    {
        var task = await _taskService.UpdateAsync(id, dto);
        return task is null ? NotFound() : Ok(task);
    }

    /// <summary>Delete a task</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _taskService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    /// <summary>Health check</summary>
    [HttpGet("/health")]
    public IActionResult Health() => Ok(new { status = "healthy", service = "TaskService" });
}
