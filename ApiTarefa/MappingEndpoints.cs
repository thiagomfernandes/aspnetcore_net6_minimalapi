using Microsoft.EntityFrameworkCore;

public static class MappingEndpoints
{
    public static void MapTarefasEndpoints(this WebApplication app)
    {
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
    }



}