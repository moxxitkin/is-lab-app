using IsLabApp;
var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();

// Диагностические эндпоинты
app.MapGet("/health", () => new { status = "ok", timestamp = DateTime.UtcNow });
app.MapGet("/version", (IConfiguration config) => new
{
    name = config["App:Name"],
    version = config["App:Version"]
});
// Простой эндпоинт для теста
app.MapGet("/", () => "Hello from IsLabApp!");

// Эндпоинт заметок (CRUD в памяти)
var notes = new List<Note>();
var nextId = 1;

app.MapGet("/api/notes", () => notes);

app.MapGet("/api/notes/{id}", (int id) =>
{
    var note = notes.FirstOrDefault(n => n.Id == id);
    return note is null ? Results.NotFound() : Results.Ok(note);
});

app.MapPost("/api/notes", (Note newNote) =>
{
    newNote.Id = nextId++;
    newNote.CreatedAt = DateTime.UtcNow;
    notes.Add(newNote);
    return Results.Created($"/api/notes/{newNote.Id}", newNote);
});

app.MapDelete("/api/notes/{id}", (int id) =>
{
    var note = notes.FirstOrDefault(n => n.Id == id);
    if (note is null) return Results.NotFound();
    notes.Remove(note);
    return Results.NoContent();
});

// Эндпоинт для проверки подключения к БД
app.MapGet("/db/ping", (IConfiguration config) =>
{
    var connectionString = config.GetConnectionString("Mssql");
    if (string.IsNullOrEmpty(connectionString))
    {
        return Results.BadRequest(new { error = "Connection string not configured" });
    }
    
    return Results.Ok(new 
    { 
        status = "ok", 
        message = "Connection string is set",
        connectionString = connectionString.Replace("YourStrong!Passw0rd", "***") 
    });
});

app.Run();

