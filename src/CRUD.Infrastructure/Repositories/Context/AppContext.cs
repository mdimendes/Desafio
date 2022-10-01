using System;
using System.IO;
using System.Linq;
using System.Reflection;
using CRUD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;



namespace CRUD.Infrastructure.Repositories.Context
{
    public class AppDbContext : DbContext
    {
        private static readonly ILoggerFactory _logger = LoggerFactory.Create(p => p.AddConsole());

        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<PedidoItem> PedidoItens { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("Appsettings.json", false, true)
            .Build();

            optionsBuilder
                //.UseLoggerFactory(_logger)
                //.EnableSensitiveDataLogging() //Exibe valores dos parametros no console de logs
                .UseSqlServer(configuration.GetConnectionString("ServerConnection"),
                p => p.EnableRetryOnFailure(
                    maxRetryCount: 2,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorNumbersToAdd: null)
                    .MigrationsAssembly("CRUD.Api")
                    .MigrationsHistoryTable("MigrationsHistoryTable"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //FLUENT API
            //Mapeia dinamicamente todas as entidades que implementam IEntityTypeConfiguration
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

             modelBuilder.Entity<Cliente>( p=>
            {
                p.ToTable("Clientes"); //Informa a tabela a ser criada
                p.HasKey(p => p.Id); //Informa a chave primaria
                p.Property(p => p.Nome).HasColumnType("VARCHAR(80)").IsRequired(); //Configura DataType e Nullability
                p.Property(p => p.Telefone).HasColumnType("CHAR(11)");
                p.Property(p => p.CEP).HasColumnType("CHAR(2)").IsRequired();
                p.Property(p => p.Estado).HasColumnType("CHAR(2)").IsRequired();
                p.Property(p => p.Cidade).HasMaxLength(60).IsRequired(); //Configura Tamanho Maximo

                p.HasIndex(i => i.Telefone).HasName("idx_cliente_telefone"); //Cria Indice
            });

            modelBuilder.Entity<Produto>(p =>
            {
                p.ToTable("Produtos");
                p.HasKey(p => p.Id);
                p.Property(p => p.CodigoBarras).HasColumnType("VARCHAR(14)").IsRequired();
                p.Property(p => p.Descricao).HasColumnType("VARCHAR(60)");
                p.Property(p => p.Valor).IsRequired();
                p.Property(p => p.TipoProduto).HasConversion<string>(); //Grava o nome do Enum
            });

            modelBuilder.Entity<Pedido>( p=>
            {
                p.ToTable("Pedidos");
                p.HasKey(p => p.Id);
                p.Property(p => p.Iniciadoem).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd(); //Configura DefaultValue no momento da geração da instrução SQL 
                p.Property(p => p.Status).HasConversion<string>();
                p.Property(p => p.TipoFrete).HasConversion<int>(); //Grava o int do Enum
                p.Property(p => p.Observacao).HasColumnType("VARCHAR(512)");

                p.HasMany(p => p.Itens) //Informa Relacionamento 1/N
                    .WithOne(p => p.Pedido)
                    .OnDelete(DeleteBehavior.Cascade); //Informa a necessidade de fazer o Delete Cascade na deleção do Pedido
            });

            modelBuilder.Entity<PedidoItem>( p=>
           {
               p.ToTable("PedidoItens");
               p.HasKey(p => p.Id);
               p.Property(p => p.Quantidade).HasDefaultValue(1).IsRequired(); //Outra Forma de inserir valor default
               p.Property(p => p.Valor).IsRequired();
               p.Property(p => p.Desconto).IsRequired();
           });

            
        }
    }

}
