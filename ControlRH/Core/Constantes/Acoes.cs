namespace ControlRH.Core.Constantes;

public static class Acoes
{
    public static class AdministradoresAcoes
    {
        public const string ControleTotal = "controle.total";
    }

    public static class PontoEletronicoAcoes
    {
        public const string MarcarPonto = "marcar.ponto";
        public const string ConsultarPonto = "consultar.ponto";
        public const string ExportarRelatorioPonto = "exportar.relatorio.ponto";
        public const string DocumentoPonto = "documento.ponto";
    }

    public static class FeriasAcoes
    {
        public const string SolicitarFerias = "solicitar.ferias";
        public const string ConsultarFerias = "consultar.ferias";
    }

    public static class AdministradorAcoes
    {
        public static class UsuariosAcoes
        {
            public const string Gerenciar = "administrador.usuarios.gerenciar";
            public const string Criar = "administrador.usuarios.criar";
            public const string Editar = "administrador.usuarios.editar";
            public const string Excluir = "administrador.usuarios.excluir";
        }

        public static class DepartamentosAcoes
        {
            public const string Gerenciar = "administrador.departamentos.gerenciar";
        }
    }

    public const string AcessoDashboardGeral = "acesso.dashboard.geral";
}
