using System.ComponentModel;

namespace ControlRH.Core.Enums;

public enum PermissaoType
{
    [Description("Usuário")]
    Usuario,

    [Description("Grupo")]
    Grupo,

    [Description("Produto")]
    Produto,

    [Description("Financeiro")]
    Financeiro,
    
    [Description("Relatório")]
    Relatorio
}
