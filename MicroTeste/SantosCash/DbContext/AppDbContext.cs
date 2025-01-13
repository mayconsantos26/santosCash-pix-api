using Microsoft.EntityFrameworkCore;
using MicroTeste.Models;

namespace Dbcontext;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Transacoes>? Transacoes { get; set; }
    public DbSet<Keys>? Keys { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        // Fluent API - Definição de regras erelacionamentos das entidades no Banco de Dados


        // Definição da entidade Transações
        mb.Entity<Transacoes>()
            .HasKey(t => t.Id); // Configurando a chave primária

        mb.Entity<Transacoes>()
            .Property(t => t.Txid) // Propriedade Txid
            .HasMaxLength(35) // Tamanho máximo de caracteres
            .IsRequired(); // Nome é obrigatório, não pode ser nulo

        mb.Entity<Transacoes>()
            .Property(t => t.E2E_Id) // Propriedade E2E_Id
            .HasMaxLength(64)
            .IsRequired();

        mb.Entity<Transacoes>()
            .Property(t => t.Pagador_Nome) // Propriedade Pagador_Nome
            .HasMaxLength(100)
            .IsRequired();

        mb.Entity<Transacoes>()
            .Property(t => t.Pagador_Cpf) // Propriedade Pagador_Cpf
            .HasMaxLength(11)
            .IsRequired();

        mb.Entity<Transacoes>()
            .Property(t => t.Pagador_Banco) // Propriedade Pagador_Banco
            .HasMaxLength(8)
            .IsRequired();

        mb.Entity<Transacoes>()
            .Property(t => t.Pagador_Agencia) // Propriedade Pagador_Agencia
            .HasMaxLength(6)
            .IsRequired();

        mb.Entity<Transacoes>()
            .Property(t => t.Pagador_Conta) // Propriedade Pagador_Conta
            .HasMaxLength(10)
            .IsRequired();

        mb.Entity<Transacoes>()
            .Property(t => t.Recebedor_Nome) // Propriedade Recebedor_Nome
            .HasMaxLength(100)
            .IsRequired();

        mb.Entity<Transacoes>()
            .Property(t => t.Recebedor_Cpf) // Propriedade Recebedor_Cpf
            .HasMaxLength(11)
            .IsRequired();

        mb.Entity<Transacoes>()
            .Property(t => t.Recebedor_Banco) // Propriedade Recebedor_Banco
            .HasMaxLength(8)
            .IsRequired();

        mb.Entity<Transacoes>()
            .Property(t => t.Recebedor_Agencia) // Propriedade Recebedor_Agencia
            .HasMaxLength(6)
            .IsRequired();

        mb.Entity<Transacoes>()
            .Property(t => t.Recebedor_Conta) // Propriedade Recebedor_Conta
            .HasMaxLength(10)
            .IsRequired();

        mb.Entity<Transacoes>()
            .Property(t => t.Valor) // Propriedade Valor
            .HasPrecision(10, 2)
            .IsRequired();

        mb.Entity<Transacoes>()
            .Property(t => t.Data_Transacao)
            .IsRequired();

        // Definição da entidade keys

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
    }
}



