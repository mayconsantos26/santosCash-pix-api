using Dbcontext;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configuração da conexão com o BD MySQL
builder.Services.AddControllersWithViews(); // MVC
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // mapeamento de entidades

string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection)));

// Registrando Services com sua implementação
builder.Services.AddScoped<ItransacoesServices, TransacoesServices>();
builder.Services.AddScoped<ITransacoesRepository, TransacoesRepository>();

// Adicionado a conexão com o mapeamento AutoMapper e perfil de mapeamento  
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
