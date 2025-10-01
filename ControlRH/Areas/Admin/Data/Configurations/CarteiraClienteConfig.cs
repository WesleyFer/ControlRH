using ControlRH.Areas.Admin.Models;
using ControlRH.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlRH.Areas.Admin.Data.Configurations;


internal class CarteiraClienteConfig : AggregateRootConfig<CarteiraCliente>, IEntityTypeConfiguration<CarteiraCliente>
{
    public override void Configure(EntityTypeBuilder<CarteiraCliente> builder)
    {
        base.Configure(builder);

        builder.ToTable("CarteiraCliente");

        builder.HasKey(c => c.Id);

        builder.Property(d => d.Nome)
           .HasMaxLength(100)
           .IsRequired();

        builder.HasMany(c => c.Colaboradores)
          .WithOne(c => c.CarteiraCliente)
          .HasForeignKey(c => c.CarteiraClienteId)
          .IsRequired();
    }
}