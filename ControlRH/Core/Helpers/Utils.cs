namespace ControlRH.Core.Helpers;

public static class Utils
{
    public static string CryptoSenha(string senha)
    {
        return BCrypt.Net.BCrypt.HashPassword(senha);
    }

    public static bool VerificaSenha(string senha, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(senha, hash);
    }

    public static string FormatarCpf(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf) || cpf.Length != 11)
            return cpf;

        return Convert.ToUInt64(cpf).ToString(@"000\.000\.000\-00");
    }

    public static string FormatarPis(string pis)
    {
        if (string.IsNullOrWhiteSpace(pis) || pis.Length != 11)
            return pis;

        return Convert.ToUInt64(pis).ToString(@"000\.00000\.00\-0");
    }

    public static string FormatarMatricula(string matricula)
    {
        if (string.IsNullOrWhiteSpace(matricula))
            return matricula;

        return matricula.PadLeft(5, '0');
    }

    public static string MascararCpfFormatado(string cpf)
    {
        if (cpf.Length != 11)
            return "CPF inválido";

        var parte1 = cpf.Substring(0, 3);      // 123
        var parte2 = cpf.Substring(3, 1);      // 4
        var parte3 = "**";
        var parte4 = "***";
        var parte5 = "**";

        return $"{parte1}.{parte2}{parte3}.{parte4}-{parte5}";
    }
}
