using ControlRH.Core.Contracts;
using ControlRH.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlRH.Core.Data
{
    public class AggregateRootConfig<T> : EntidadeConfig<T> where T : Entidade, IAggregateRoot
    {
        public override void Configure(EntityTypeBuilder<T> builder)
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