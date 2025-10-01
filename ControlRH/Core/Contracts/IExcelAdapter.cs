using ControlRH.Core.Models;

namespace ControlRH.Core.Contracts
{
    public interface IExcelAdapter
    {
        byte[] GerarExcel<T>(string nomeAba, string titulo, Dictionary<string, object> cabecalho, List<ExcelColunaInfo> colunas, IEnumerable<T> dados);
    }
}