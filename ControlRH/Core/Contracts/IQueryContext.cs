using ControlRH.Areas.Admin.Models;
using ControlRH.Areas.Colaborador.Models;

namespace ControlRH.Core.Contracts;

public interface IQueryContext
{
    IQueryable<Usuario> QueryUsuarios { get; }

    IQueryable<Grupo> QueryGrupos { get; }

    IQueryable<Permissao> QueryPermissoes { get; }

    IQueryable<CarteiraCliente> QueryCarteirasClientes { get; }

    IQueryable<Colaborador> QueryColaboradores { get; }

    IQueryable<JornadaTrabalho> QueryJornadasTrabalhos { get; }

    IQueryable<PontoEletronico> QueryPontosEletronicos { get; }

    IQueryable<Cargo> QueryCargos { get; }

    IQueryable<Documento> QueryDocumentos { get; }

}