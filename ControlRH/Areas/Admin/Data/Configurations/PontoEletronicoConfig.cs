using ControlRH.Areas.Admin.Models;
using ControlRH.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlRH.Areas.Admin.Data.Configurations;

internal class PontoEletronicoConfig : AggregateRootConfig<PontoEletronico>, IEntityTypeConfiguration<PontoEletronico>
{
    public override void Configure(EntityTypeBuilder<PontoEletronico> builder)
    {
        builder.ToTable("PontoEletronico");

        builder.HasKey(c => c.Id);
    }
}