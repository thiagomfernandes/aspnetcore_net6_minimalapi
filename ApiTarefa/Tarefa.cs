using Microsoft.EntityFrameworkCore;

class Tarefa
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public bool IsConcluida { get; set; }
}

class ApiTarefasDbContext : DbContext
{
    public ApiTarefasDbContext(DbContextOptions<ApiTarefasDbContext> options) : base(options)
    { }

    public DbSet<Tarefa> Tarefas => Set<Tarefa>();
}