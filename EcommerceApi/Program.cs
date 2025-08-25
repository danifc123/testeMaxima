using EcommerceApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Adiciona os controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adiciona CORS para permitir requisições do frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configuração da conexão com o banco
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registra os repositórios
builder.Services.AddScoped<IProdutoRepository>(provider => new ProdutoRepository(connectionString!));
builder.Services.AddScoped<IUsuarioRepository>(provider => new UsuarioRepository(connectionString!));

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Usa CORS
app.UseCors("AllowAll");

app.UseAuthorization();

// Mapeia os controllers
app.MapControllers();

app.Run();
