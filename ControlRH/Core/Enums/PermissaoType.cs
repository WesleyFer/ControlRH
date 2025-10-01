using System.ComponentModel;

namespace ControlRH.Core.Enums;

public enum PermissaoType
{
    [Description("Sistema")]
    Sistema,

    [Description("Ponto Eletrônico")]
    PontoEletronico,

    [Description("Férias")]
    Ferias,

    [Description("Relatório")]
    Relatorio
}
