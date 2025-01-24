using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MicroTeste.Models;

namespace Dbcontext;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Transacoes>? Transacoes { get; set; }
    public DbSet<Keys>? Keys { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        // Fluent API - Definição de regras e relacionamentos das entidades no Banco de Dados

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
            .Property(t => t.Pagador_Cpf) // Propriedade Pagador_Cpf
            .HasMaxLength(11);

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
            .Property(t => t.Recebedor_Cpf) // Propriedade Recebedor_Cpf
            .HasMaxLength(11);

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
            .Property(t => t.Data_Transacao) // Propriedade Data_Transacao
            .IsRequired();

        // Definição da entidade Keys

        mb.Entity<Keys>()
            .Property(a => a.Id); // Definindo chave primária

        mb.Entity<Keys>()
            .Property(a => a.ApiKey) // Propriedade ApiKey
            .HasMaxLength(64)
            .IsRequired();

        mb.Entity<Keys>()
            .Property(a => a.Nome) // Propriedade Nome
            .HasMaxLength(100)
            .IsRequired();

        mb.Entity<Keys>()
            .Property(a => a.Cnpj) // Propriedade Cnpj
            .HasMaxLength(14)
            .IsRequired();

        mb.Entity<Keys>()
            .Property(a => a.Conta) // Propriedade Conta
            .HasMaxLength(10)
            .IsRequired();

        // Converte todas as propriedades DateTime para UTC ao salvar no banco
        foreach (var property in mb.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(DateTime)))
        {
            property.SetValueConverter(new ValueConverter<DateTime, DateTime>(
                v => v.ToUniversalTime(), // Converte para UTC antes de salvar
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc) // Garantir que é UTC ao ler
            ));
        }
    }
}
