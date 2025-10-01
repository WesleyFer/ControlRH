using System.ComponentModel;

namespace ControlRH.Core.Enums;

public enum MarcacaoType
{
    [Description("Início de Expediente")]
    Entrada = 1,

    [Description("Saída para Almoço/Intervalo")]
    SaidaIntervalo = 2,

    [Description("Retorno do Almoço/Intervalo")]
    RetornoIntervalo = 3,

    [Description("Término de Expediente")]
    Saida = 4
}