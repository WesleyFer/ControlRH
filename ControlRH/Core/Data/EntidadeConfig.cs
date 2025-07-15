using ControlRH.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlRH.Core.Data
{
    public class EntidadeConfig<T> : IEntityTypeConfiguration<T> where T : Entidade
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(e => e.DataCriacao)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(e => e.DataAlteracao)
                .ValueGeneratedOnAddOrUpdate()
                .IsRequired();
        }
    }
}