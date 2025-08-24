var builder = WebApplication.CreateBuilder(args);

// Adiciona os controllers
builder.Services.AddControllers();          // <- ESSENCIAL
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Mapeia os controllers
app.MapControllers();                       // <- ESSENCIAL

app.Run();
