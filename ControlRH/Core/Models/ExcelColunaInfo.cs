namespace ControlRH.Core.Models
{
    public class ExcelColunaInfo
    {
        public string NomeColuna { get; set; }
        public string Propriedade { get; set; }
        public string FormatoExcel { get; set; } // Ex: "dd/MM/yyyy", "R$ #,##0.00"
    }
}