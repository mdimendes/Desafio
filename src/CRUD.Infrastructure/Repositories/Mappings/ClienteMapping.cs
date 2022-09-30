using CRUD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRUD.Infrastructure.Repositories.Mappings
{
    public class ClienteMapping: IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Clientes"); //Informa a tabela a ser criada
            builder.HasKey(p => p.Id); //Informa a chave primaria
            builder.Property(p => p.Nome).HasColumnType("VARCHAR(80)").IsRequired(); //Configura DataType e Nullability
            builder.Property(p => p.Telefone).HasColumnType("CHAR(11)");
            builder.Property(p => p.CEP).HasColumnType("CHAR(2)").IsRequired();
            builder.Property(p => p.Estado).HasColumnType("CHAR(2)").IsRequired();
            builder.Property(p => p.Cidade).HasMaxLength(60).IsRequired(); //Configura Tamanho Maximo

            builder.HasIndex(i => i.Telefone).HasName("idx_cliente_telefone"); //Cria Indice
        }
    }
}