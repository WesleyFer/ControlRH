using ControlRH.Areas.Admin.Models;
using ControlRH.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlRH.Areas.Admin.Data.Configurations;

internal class UsuarioGrupoConfig : EntidadeConfig<UsuarioGrupo>, IEntityTypeConfiguration<UsuarioGrupo>
{
    public override void Configure(EntityTypeBuilder<UsuarioGrupo> builder)
    {
        base.Configure(builder);

        builder.ToTable("UsuarioGrupo");

        builder.HasKey(c => c.Id);

        builder.HasOne(c => c.Usuario)
            .WithMany(c => c.UsuariosGrupos)
            .HasForeignKey(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(c => c.Grupo)
            .WithMany(c => c.UsuariosGrupos)
            .HasForeignKey(c => c.GrupoId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}