using ControlRH.Areas.Admin.Models;
using ControlRH.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlRH.Areas.Admin.Data.Configurations;

internal class GrupoPermissaoConfig : EntidadeConfig<GrupoPermissao>, IEntityTypeConfiguration<GrupoPermissao>
{
    public override void Configure(EntityTypeBuilder<GrupoPermissao> builder)
    {
        base.Configure(builder);

        builder.ToTable("GrupoPermissao");

        builder.HasKey(c => c.Id);

        builder.HasOne(c => c.Grupo)
            .WithMany(c => c.GruposPermissoes)
            .HasForeignKey(c => c.GrupoId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(c => c.Permissao)
            .WithMany(c => c.GruposPermissoes)
            .HasForeignKey(c => c.PermissaoId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}