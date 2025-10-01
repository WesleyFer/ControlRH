using ControlRH.Areas.Admin.Models;
using ControlRH.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlRH.Areas.Admin.Data.Configurations;

internal class GrupoConfig : AggregateRootConfig<Grupo>, IEntityTypeConfiguration<Grupo>
{
    public override void Configure(EntityTypeBuilder<Grupo> builder)
    {
        base.Configure(builder);

        builder.ToTable("Grupo");

        builder.HasKey(c => c.Id);

        builder.Property(g => g.Nome)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(g => g.Tipo)
            .HasConversion<string>()
            .IsRequired();

        builder.HasMany(c => c.UsuariosGrupos)
            .WithOne(c => c.Grupo)
            .HasForeignKey(c => c.GrupoId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasMany(c => c.GruposPermissoes)
            .WithOne(c => c.Grupo)
            .HasForeignKey(c => c.GrupoId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}