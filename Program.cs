using Microsoft.EntityFrameworkCore;
using ToDoListApi;
using ToDoListApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

// API for TaskHub models
// WorkSpace
app.MapGet("/workspaces", async (TodoDb db) =>
    await db.WorkSpaces
        .AsNoTracking()
        .OrderByDescending(i => i.WorkSpaceId)
        .ToListAsync());

app.MapGet("/workspaces/{id}", async (int id, TodoDb db) =>
    await db.WorkSpaces.FindAsync(id)
        is WorkSpace workSpace
        ? Results.Ok(workSpace) : Results.NotFound());

app.MapPost("/workspaces", async (WorkSpace workpace, TodoDb db) =>
{
    db.WorkSpaces.Add(workpace);
    await db.SaveChangesAsync();

    return Results.Created($"/workspaces/{workpace.WorkSpaceId}", workpace);
});

app.MapPut("/workspaces/{id}", async (int id, WorkSpace inputTodo, TodoDb db) =>
{
    var workpace = await db.WorkSpaces.FindAsync(id);
    if (workpace == null) return Results.NotFound();

    workpace.WorkSpaceTitle = inputTodo.WorkSpaceTitle;
    workpace.WorkSpaceDescription = inputTodo.WorkSpaceDescription;
    workpace.UserId = inputTodo.UserId;
    workpace.Status = inputTodo.Status;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/workspaces/{id}", async (int id, TodoDb db) =>
{
    if (await db.WorkSpaces.FindAsync(id) is WorkSpace workpace)
    {
        db.WorkSpaces.Remove(workpace);
        await db.SaveChangesAsync();
        return Results.Ok(workpace);
    }
    return Results.NotFound();
});

// Todo items for the test
app.MapGet("/todoitems", async (TodoDb db) =>
    await db.Todos.ToListAsync());

app.MapGet("/todoitems/complete", async (TodoDb db) =>
    await db.Todos.Where(t => t.IsComplete).ToListAsync());

app.MapGet("/todoitems/{id}", async (int id, TodoDb db) =>
    await db.Todos.FindAsync(id)
        is Todo toDo
        ? Results.Ok(toDo) : Results.NotFound());

app.MapPost("/todoitems", async (Todo todo, TodoDb db) =>
{
    db.Todos.Add(todo);
    await db.SaveChangesAsync();

    return Results.Created($"/todoitems/{todo.Id}", todo);
});

app.MapPut("/todoitems/{id}", async (int id, Todo inputTodo, TodoDb db) =>
{
    var todo = await db.Todos.FindAsync(id);
    if (todo == null) return Results.NotFound();

    todo.Name = inputTodo.Name;
    todo.IsComplete = inputTodo.IsComplete;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/todoitems/{id}", async (int id, TodoDb db) =>
{
    if (await db.Todos.FindAsync(id) is Todo todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return Results.Ok(todo);
    }
    return Results.NotFound();
});
app.Run();
