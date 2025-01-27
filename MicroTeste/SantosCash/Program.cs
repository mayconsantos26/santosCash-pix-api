using System.Text;
using AutoMapper;
using Dbcontext;
using DTOs.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using Services;

var builder = WebApplication.CreateBuilder(args);

// **** Builder Services sendo utilizado como container de injeção de dependência ****// 

builder.Services.AddControllersWithViews(); // MVC

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Minha API",
        Version = "v1",
        Description = "Exemplo de configuração do Swagger em ASP.NET Core",
    });
});

// Definindo a política CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowedMethodsPolicy", builder =>
    {
        builder.WithOrigins("https://localhost:5001/swagger/index.html") // Substitua pelo domínio permitido
               .WithMethods("GET", "POST", "PUT", "DELETE") // Permite apenas esses métodos
               .AllowAnyHeader(); // Permite qualquer cabeçalho
    });
});

// Configurando o serviço de autenticação com JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication("Bearer")
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
    };
});

builder.Services.AddAuthorization();

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

app.UseCors("AllowedMethodsPolicy"); // CORS sempre entre UseAuthorization e UseRouting

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapControllers();

app.Run();
