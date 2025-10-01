using ControlRH.Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;

namespace ControlRH.Core.Extensions;

public static class EntityTypeBuilderExtensions
{
    public static void AdicionaDatasAutogeradas<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : Entidade
    {
        builder.Property(e => e.DataCriacao)
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)")
            .ValueGeneratedOnAdd()
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

        builder.Property(e => e.DataAlteracao)
            .ValueGeneratedOnAddOrUpdate()
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
    }
}
