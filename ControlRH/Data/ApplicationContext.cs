using ControlRH.Areas.Admin.Data.Configurations;
using ControlRH.Areas.Admin.Models;
using ControlRH.Core.Contracts;
using ControlRH.Core.Models;
using Flunt.Notifications;
using Microsoft.EntityFrameworkCore;

namespace ControlRH.Data
{
    public class ApplicationContext : DbContext, IQueryContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; protected set; }
        public DbSet<Grupo> Grupos { get; protected set; }
        public DbSet<Permissao> Permissoes { get; protected set; }
        public DbSet<UsuarioGrupo> UsuariosGrupos { get; protected set; }
        public DbSet<GrupoPermissao> GruposPermissoes { get; protected set; }
        public DbSet<Colaborador> Colaboradores { get; protected set; }
        public DbSet<JornadaTrabalho> JornadasTrabalhos { get; protected set; }
        public DbSet<ColaboradorJornada> ColaboradoresJornadas { get; protected set; }
        public DbSet<CarteiraCliente> CarteirasClientes { get; protected set; }
        public DbSet<PontoEletronico> PontosEletronicos { get; protected set; }
        public DbSet<PontoMarcacao> MarcacoesPontos { get; protected set; }
        public DbSet<PontoMarcacaoAjuste> AjustesMarcacoes { get; protected set; }

        public IQueryable<Usuario> QueryUsuarios => Usuarios
            .Include(c => c.UsuariosGrupos)
                .ThenInclude(c => c.Grupo)
                    .ThenInclude(c => c.GruposPermissoes)
                        .ThenInclude(c => c.Permissao)
            .Include(c => c.Colaborador)
            .AsNoTrackingWithIdentityResolution()
            .AsQueryable();

        public IQueryable<Grupo> QueryGrupos => Grupos
            .Include(c => c.UsuariosGrupos)
            .Include(c => c.GruposPermissoes)
                .ThenInclude(c => c.Permissao)
            .AsNoTrackingWithIdentityResolution()
            .AsQueryable();

        public IQueryable<Permissao> QueryPermissoes => Permissoes
           .AsNoTrackingWithIdentityResolution()
           .AsQueryable();

        public IQueryable<CarteiraCliente> QueryCarteirasClientes => CarteirasClientes
            .AsNoTrackingWithIdentityResolution()
            .AsQueryable();

        public IQueryable<Colaborador> QueryColaboradores => Colaboradores
            .Include(c => c.CarteiraCliente)
            .AsNoTrackingWithIdentityResolution()
            .AsQueryable();

        public IQueryable<JornadaTrabalho> QueryJornadasTrabalhos => JornadasTrabalhos
            .Include(c => c.JornadasTrabalhosHorarios)
            .AsNoTrackingWithIdentityResolution()
            .AsQueryable();

        public IQueryable<PontoEletronico> QueryPontosEletronicos => PontosEletronicos
            .AsNoTrackingWithIdentityResolution()
            .AsQueryable();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly, type =>
            {
                var mapeamentos = string.Join('.', typeof(ColaboradorConfig).FullName?.Split('.').SkipLast(1));
                return type.FullName?.StartsWith(mapeamentos) ?? false;
            });

            modelBuilder.Ignore<Notification>();
            modelBuilder.Ignore<Notifiable<Notification>>();
            modelBuilder.Ignore<Entidade>();
            modelBuilder.Ignore<IAggregateRoot>();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var agora = DateTime.Now;

            foreach (var entry in ChangeTracker.Entries<Entidade>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property(nameof(Entidade.DataCriacao)).CurrentValue = agora;
                    entry.Property(nameof(Entidade.DataAlteracao)).CurrentValue = agora;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(nameof(Entidade.DataAlteracao)).CurrentValue = agora;
                    entry.Property(nameof(Entidade.DataCriacao)).IsModified = false;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            var agora = DateTime.Now;

            foreach (var entry in ChangeTracker.Entries<Entidade>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property(nameof(Entidade.DataCriacao)).CurrentValue = agora;
                    entry.Property(nameof(Entidade.DataAlteracao)).CurrentValue = agora;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(nameof(Entidade.DataAlteracao)).CurrentValue = agora;
                    entry.Property(nameof(Entidade.DataCriacao)).IsModified = false;
                }
            }

            return base.SaveChanges();
        }
    }
}
