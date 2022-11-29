using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

class Tarefa
{
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength=5, ErrorMessage="Nome da tarefa deve ter entre 5 e 100 caracteres")]
    public string? Nome { get; set; }
    public bool IsConcluida { get; set; }
}

class ApiTarefasDbContext : DbContext
{
    public ApiTarefasDbContext(DbContextOptions<ApiTarefasDbContext> options) : base(options)
    { }

    public DbSet<Tarefa> Tarefas => Set<Tarefa>();
}