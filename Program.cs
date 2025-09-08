using adea_solution_web_api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrar el servicio de datos en memoria
builder.Services.AddSingleton<IDataService, InMemoryDataService>();

// Configurar CORS para permitir comunicación con el frontend Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200",
                "https://localhost:4200",
                "https://adeatest.netlify.app"
            )
            .WithMethods("GET", "POST", "PUT", "DELETE")
            .WithHeaders("Content-Type", "Authorization", "X-Requested-With", "Accept")
            .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

// Habilitar Swagger solo en Development y Staging
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Usar CORS
// Usar CORS antes de Authorization y MapControllers
app.UseCors("AllowAngularApp");

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
