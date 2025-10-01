namespace ControlRH.Core.Extensions;

public static class StringExtensions
{
    public static string CapitalizeFirst(this string texto)
    {
        return string.IsNullOrEmpty(texto)
            ? texto
            : char.ToUpper(texto[0]) + texto.Substring(1).ToLower();
    }
}