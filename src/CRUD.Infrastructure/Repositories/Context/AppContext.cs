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
        }
    }

}
