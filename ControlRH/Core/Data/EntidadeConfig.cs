using ControlRH.Core.Extensions;
using ControlRH.Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlRH.Core.Data;

public abstract class EntidadeConfig<TEntity> where TEntity : Entidade
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .IsRequired();

        builder.AdicionaDatasAutogeradas();
    }
}