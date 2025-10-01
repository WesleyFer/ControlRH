using ControlRH.Core.Enums;
using ControlRH.Core.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ControlRH.Areas.Colaborador.Models;

public class PontoEletronico : AggregateRoot
{
    private readonly List<AjusteMarcacao> _ajustesMarcacoes = new();

    protected PontoEletronico() { }

    public PontoEletronico(DateTime dataHoraRecebida, MarcacaoType marcacao)
    {
        DateTime agoraCompleta = DateTime.Now;

        DateTime agoraTruncada = new DateTime(
            agoraCompleta.Year,
            agoraCompleta.Month,
            agoraCompleta.Day,
            agoraCompleta.Hour,
            agoraCompleta.Minute,
            0
        );

        DateTime dataHoraRecebidaTruncada = new DateTime(
            dataHoraRecebida.Year,
            dataHoraRecebida.Month,
            dataHoraRecebida.Day,
            dataHoraRecebida.Hour,
            dataHoraRecebida.Minute,
            0
        );

        if (dataHoraRecebidaTruncada < agoraTruncada)
        {
            DataHora = agoraCompleta;
        }
        else
        {
            DataHora = dataHoraRecebida;
        }

        Marcacao = marcacao;
        HashUnico = GerarHash(DataHora, Marcacao);
    }

    public PontoEletronico(string cpf, string pis, DateTime dataHoraRecebida, MarcacaoType marcacao)
    {
        DateTime agoraCompleta = DateTime.Now;

        DateTime agoraTruncada = new DateTime(
            agoraCompleta.Year,
            agoraCompleta.Month,
            agoraCompleta.Day,
            agoraCompleta.Hour,
            agoraCompleta.Minute,
            0
        );

        DateTime dataHoraRecebidaTruncada = new DateTime(
            dataHoraRecebida.Year,
            dataHoraRecebida.Month,
            dataHoraRecebida.Day,
            dataHoraRecebida.Hour,
            dataHoraRecebida.Minute,
            0
        );

        if (dataHoraRecebidaTruncada < agoraTruncada)
        {
            DataHora = agoraCompleta;
        }
        else
        {
            DataHora = dataHoraRecebida;
        }

        Marcacao = marcacao;
        Cpf = Regex.Replace(cpf, @"\D", "");
        Pis = Regex.Replace(pis, @"\D", "");
        HashUnico = GerarHash(DataHora, Marcacao);
    }

    public string Cpf { get; private set; }

    public string Pis { get; private set; }

    public DateTime DataHora { get; private set; }

    public MarcacaoType Marcacao { get; private set; }

    public string? EnderecoIp { get; private set; }

    public string? Hostname { get; private set; }

    public string HashUnico { get; private set; } // Nova propriedade para o hash

    public IReadOnlyCollection<AjusteMarcacao> AjustesMarcacoes => _ajustesMarcacoes.AsReadOnly();

    public void AdicionarCpf(string cpf)
    {
        Cpf = cpf;
    }

    public void AdicionarPis(string pis)
    {
        Pis = pis;
    }

    public void AdicionarTipoMarcacao(MarcacaoType marcacao)
    {
        Marcacao = marcacao;
    }

    public void AdicionarEnderecoIp(string enderecoIp)
    {
        EnderecoIp = enderecoIp;
    }

    public void AdicionarHostname(string hostname)
    {
        Hostname = hostname;
    }

    // Método para gerar o hash
    private string GerarHash(DateTime dataHora, MarcacaoType tipoMarcacao)
    {
        // Concatena os dados que formam a "identidade" única da marcação
        string dadosParaHash = $"{dataHora.ToString("yyyyMMddHHmmssfff")}-{tipoMarcacao}";

        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash
                .ComputeHash(Encoding.UTF8.GetBytes(dadosParaHash));

            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2")); // Formato hexadecimal
            }

            return builder.ToString();
        }
    }

}