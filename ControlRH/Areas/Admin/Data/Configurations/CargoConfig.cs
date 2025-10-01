using ControlRH.Areas.Admin.Models;
using ControlRH.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlRH.Areas.Admin.Data.Configurations;


internal class CargoConfig : AggregateRootConfig<Cargo>, IEntityTypeConfiguration<Cargo>
{
    public override void Configure(EntityTypeBuilder<Cargo> builder)
    {
        base.Configure(builder);

        builder.ToTable("Cargo");

        builder.HasKey(c => c.Id);

        builder.Property(d => d.Nome)
           .HasMaxLength(100)
           .IsRequired();

        builder.HasMany(c => c.Colaboradores)
          .WithOne(c => c.Cargo)
          .HasForeignKey(c => c.CargoId)
          .IsRequired();
    }
}