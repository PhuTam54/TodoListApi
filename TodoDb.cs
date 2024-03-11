using Microsoft.EntityFrameworkCore;
using ToDoListApi.Models;

namespace ToDoListApi
{
    public class TodoDb : DbContext
    {
        public TodoDb(DbContextOptions<TodoDb> options) : base(options) { }
        public DbSet<Todo> Todos => Set<Todo>();
        public DbSet<WorkSpace> WorkSpaces => Set<WorkSpace>();
    }
}
