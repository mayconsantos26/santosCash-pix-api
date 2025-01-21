using AutoMapper;
using Dbcontext;
using DTOs.Mappings;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configuração da conexão com o BD MySQL
builder.Services.AddControllersWithViews(); // MVC
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // mapeamento de entidades

builder.Services.AddSwaggerGen(); // Adiciona o Swagger

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Minha API",
        Version = "v1",
        Description = "Exemplo de configuração do Swagger em ASP.NET Core",
    });
});

// Adicionado a conexão com o BD Postgres
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

// Registrando Services com sua implementação
builder.Services.AddScoped<ITransacoesServices, TransacoesServices>();
builder.Services.AddScoped<ITransacoesRepository, TransacoesRepository>();

// Adicionado a conexão com o mapeamento AutoMapper e perfil de mapeamento
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(typeof(MappingProfile));

var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<MappingProfile>(); // Adiciona o perfil que define o mapeamento
});

var app = builder.Build();

// Configuração do Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapControllers();

app.Run();
