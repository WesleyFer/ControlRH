using ControlRH.Core.Contracts;
using ControlRH.Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlRH.Core.Data;

public abstract class AggregateRootConfig<T> : EntidadeConfig<T> where T : AggregateRoot, IAggregateRoot
{
    public override void Configure(EntityTypeBuilder<T> builder)
    {
        base.Configure(builder);
    }
}