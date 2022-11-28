using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApiTarefasDbContext>(opt =>
    opt.UseInMemoryDatabase("TarefasDB")
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/helloworld", () => "Hello World");

app.MapGet("/randomfrases", async () =>
    await new HttpClient().GetStringAsync("https://ron-swanson-quotes.herokuapp.com/v2/quotes")
);

app.MapGet("/tarefas/{id}", async (int id, ApiTarefasDbContext db) =>
    {
        var tarefa = await db.Tarefas.FindAsync(id);
        if (tarefa is Tarefa)
            return Results.Ok(tarefa);
        else
            return Results.NotFound();
    }
);

app.MapGet("/tarefas/concluidas", async (ApiTarefasDbContext db) =>
{
    var concluidas = await db.Tarefas.Where(t => t.IsConcluida).ToListAsync();
    return concluidas;
}
);

app.MapGet("/tarefas", async (ApiTarefasDbContext db) =>
{
    return await db.Tarefas.ToListAsync();
}
);

app.MapPut("/tarefas/{id}", async (int id, Tarefa input, ApiTarefasDbContext db) =>
{
    var tarefa = await db.Tarefas.FindAsync(id);
    if (tarefa is not Tarefa)
        return Results.NotFound();
    
    tarefa.Nome = input.Nome;
    tarefa.IsConcluida = input.IsConcluida;

    await db.SaveChangesAsync();
    return Results.NoContent();
        
}
);

app.MapDelete("/tarefas/{id}", async (int id, ApiTarefasDbContext db) =>
{
    var tarefa = await db.Tarefas.FindAsync(id);
    if (tarefa is not Tarefa)
        return Results.NotFound();

    db.Tarefas.Remove(tarefa);
    await db.SaveChangesAsync();

    return Results.Ok(tarefa);
        
}
);

app.MapPost("/tarefas", async (Tarefa tarefa, ApiTarefasDbContext db) =>
    {
        db.Tarefas.Add(tarefa);
        await db.SaveChangesAsync();
        return Results.Created($"/tarefas/{tarefa.Id}", tarefa);
    }
);


app.Run();

class Tarefa
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public bool IsConcluida { get; set; }
}

class ApiTarefasDbContext: DbContext
{
    public ApiTarefasDbContext(DbContextOptions<ApiTarefasDbContext> options) : base(options)
    { }

    public DbSet<Tarefa> Tarefas => Set<Tarefa>();
}