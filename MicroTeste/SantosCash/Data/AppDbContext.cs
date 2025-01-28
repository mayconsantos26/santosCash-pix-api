using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Models;

namespace Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
    { 
    }

    public DbSet<Transacoes>? Transacoes { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        // Fluent API - Definição de regras e relacionamentos das entidades no Banco de Dados

        base.OnModelCreating(mb); // Chama o método OnModelCreating da classe base

        // Definição da entidade Transações
        mb.Entity<Transacoes>()
            .HasKey(t => t.Id); // Configurando a chave primária

        mb.Entity<Transacoes>()
            .Property(t => t.Txid) // Propriedade Txid
            .HasMaxLength(35) // Tamanho máximo de caracteres
            .IsRequired(); // Nome é obrigatório, não pode ser nulo

        mb.Entity<Transacoes>()
            .Property(t => t.E2E_Id) // Propriedade E2E_Id
            .HasMaxLength(64);

        mb.Entity<Transacoes>()
            .Property(t => t.Pagador_Nome) // Propriedade Pagador_Nome
            .HasMaxLength(100);

        mb.Entity<Transacoes>()
            .Property(t => t.Pagador_Documento) // Propriedade Pagador_Documento
            .HasMaxLength(14);

        mb.Entity<Transacoes>()
            .Property(t => t.Pagador_Banco) // Propriedade Pagador_Banco
            .HasMaxLength(8);

        mb.Entity<Transacoes>()
            .Property(t => t.Pagador_Agencia) // Propriedade Pagador_Agencia
            .HasMaxLength(6);

        mb.Entity<Transacoes>()
            .Property(t => t.Pagador_Conta) // Propriedade Pagador_Conta
            .HasMaxLength(10);

        mb.Entity<Transacoes>()
            .Property(t => t.Recebedor_Nome) // Propriedade Recebedor_Nome
            .HasMaxLength(100);

        mb.Entity<Transacoes>()
            .Property(t => t.Recebedor_Documento) // Propriedade Recebedor_Documento
            .HasMaxLength(14);

        mb.Entity<Transacoes>()
            .Property(t => t.Recebedor_Banco) // Propriedade Recebedor_Banco
            .HasMaxLength(8);

        mb.Entity<Transacoes>()
            .Property(t => t.Recebedor_Agencia) // Propriedade Recebedor_Agencia
            .HasMaxLength(6);

        mb.Entity<Transacoes>()
            .Property(t => t.Recebedor_Conta) // Propriedade Recebedor_Conta
            .HasMaxLength(10);

        mb.Entity<Transacoes>()
            .Property(t => t.Valor) // Propriedade Valor
            .HasPrecision(10, 2)
            .IsRequired();

        mb.Entity<Transacoes>()
        .Property(t => t.Data_Transacao)
        .HasPrecision(3) // Exemplo de precisão (opcional)
        .IsRequired();

        // Configuração global para DateTime e DateTime? como UTC
        foreach (var property in mb.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?)))
        {
            property.SetValueConverter(new ValueConverter<DateTime, DateTime>(
                v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            ));
        }
    }
}
