using System.Drawing;
using ControlRH.Core.Contracts;
using ControlRH.Core.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ControlRH.Core.Adapters
{
    public class ExcelAdapter : IExcelAdapter
    {
        public byte[] GerarExcel<T>(string nomeAba, string titulo, Dictionary<string, object> cabecalho, List<ExcelColunaInfo> colunas, IEnumerable<T> dados)
        {
            using var package = new ExcelPackage();
            var aba = package.Workbook.Worksheets.Add(nomeAba);

            int linhaAtual = 1;

            linhaAtual = AdicionarTitulo(aba, titulo, colunas.Count, linhaAtual);
            linhaAtual++; // linha em branco

            linhaAtual = AdicionarCabecalho(aba, cabecalho, linhaAtual);
            linhaAtual++;

            linhaAtual = AdicionarColunas(aba, colunas, linhaAtual);
            linhaAtual++;

            AdicionarDados(aba, colunas, dados, linhaAtual);

            aba.Cells[aba.Dimension.Address].AutoFitColumns();

            return package.GetAsByteArray();
        }

        private int AdicionarTitulo(ExcelWorksheet aba, string titulo, int totalColunas, int linhaAtual)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                return linhaAtual;

            var cell = aba.Cells[linhaAtual, 1];
            cell.Value = titulo;
            cell.Style.Font.Bold = true;
            cell.Style.Font.Size = 16;
            cell.Style.Font.Color.SetColor(Color.FromArgb(68, 114, 196));
            aba.Cells[linhaAtual, 1, linhaAtual, totalColunas].Merge = true;

            return linhaAtual + 1;
        }

        private int AdicionarCabecalho(ExcelWorksheet aba, Dictionary<string, object> cabecalho, int linhaAtual)
        {
            if (cabecalho == null)
                return linhaAtual;

            foreach (var item in cabecalho)
            {
                aba.Cells[linhaAtual, 1].Value = item.Key;
                aba.Cells[linhaAtual, 1].Style.Font.Bold = true;
                aba.Cells[linhaAtual, 2].Value = item.Value;
                linhaAtual++;
            }

            return linhaAtual;
        }

        private int AdicionarColunas(ExcelWorksheet aba, List<ExcelColunaInfo> colunas, int linhaAtual)
        {
            for (int i = 0; i < colunas.Count; i++)
            {
                var cell = aba.Cells[linhaAtual, i + 1];
                cell.Value = colunas[i].NomeColuna;
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 11;
                cell.Style.Font.Name = "Aptos Narrow";
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(166, 201, 236));
            }

            aba.Row(linhaAtual).Height = 18;
            aba.Cells[linhaAtual, 1, linhaAtual, colunas.Count].AutoFilter = true;

            return linhaAtual + 1;
        }

        private void AdicionarDados<T>(ExcelWorksheet aba, List<ExcelColunaInfo> colunas, IEnumerable<T> dados, int linhaAtual)
        {
            foreach (var item in dados)
            {
                for (int i = 0; i < colunas.Count; i++)
                {
                    var prop = typeof(T).GetProperty(colunas[i].Propriedade);
                    var valor = prop?.GetValue(item);
                    var cell = aba.Cells[linhaAtual, i + 1];

                    cell.Value = valor;

                    if (!string.IsNullOrWhiteSpace(colunas[i].FormatoExcel))
                    {
                        cell.Style.Numberformat.Format = colunas[i].FormatoExcel;
                    }

                    cell.Style.Font.Name = "Aptos Narrow";
                    cell.Style.Font.Size = 10;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;

                    // Bordas
                    var border = cell.Style.Border;
                    border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    border.Top.Color.SetColor(Color.LightGray);
                }

                aba.Row(linhaAtual).Height = 16;
                linhaAtual++;
            }
        }

        //public byte[] GerarExcel<T>(string nomeAba, string titulo, Dictionary<string, object> cabecalho, List<ExcelColumnInfo> colunas, IEnumerable<T> dados)
        //{
        //    using var package = new ExcelPackage();

        //    var aba = package.Workbook.Worksheets.Add(nomeAba);

        //    // Título
        //    if (!string.IsNullOrWhiteSpace(titulo))
        //    {
        //        aba.Cells["A1"].Value = titulo;
        //        aba.Cells["A1"].Style.Font.Size = 16;
        //        aba.Cells["A1"].Style.Font.Bold = true;
        //        aba.Cells[1, 1, 1, colunas.Count].Merge = true;
        //    }

        //    // Cabeçalho (A2 até A6 por exemplo)
        //    int linha = 2;
        //    foreach (var item in cabecalho)
        //    {
        //        aba.Cells[linha, 1].Value = item.Key;
        //        aba.Cells[linha, 2].Value = item.Value;
        //        linha++;
        //    }

        //    // Colunas
        //    int linhaColunas = linha + 1;
        //    for (int i = 0; i < colunas.Count; i++)
        //    {
        //        aba.Cells[linhaColunas, i + 1].Value = colunas[i].NomeColuna;
        //        aba.Cells[linhaColunas, i + 1].Style.Font.Bold = true;
        //    }

        //    // Dados
        //    int linhaDados = linhaColunas + 1;
        //    foreach (var registro in dados)
        //    {
        //        for (int i = 0; i < colunas.Count; i++)
        //        {
        //            var prop = typeof(T).GetProperty(colunas[i].Propriedade);
        //            var valor = prop?.GetValue(registro);
        //            var cell = aba.Cells[linhaDados, i + 1];
        //            cell.Value = valor;

        //            if (!string.IsNullOrEmpty(colunas[i].FormatoExcel))
        //            {
        //                cell.Style.Numberformat.Format = colunas[i].FormatoExcel;
        //            }
        //        }
        //        linhaDados++;
        //    }

        //    aba.Cells[aba.Dimension.Address].AutoFitColumns();

        //    return package.GetAsByteArray();
        //}
    }
}
