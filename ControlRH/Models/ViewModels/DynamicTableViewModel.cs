namespace ControlRH.Models.ViewModels
{
    public class DynamicTableViewModel
    {
        public Dictionary<string, string> Columns { get; set; } = new();
        public IEnumerable<object> Data { get; set; } = Enumerable.Empty<object>();
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalItems { get; set; }
        public string? Search { get; set; }
        public string? Sort { get; set; }
        public string? Dir { get; set; }
        public string? AreaName { get; set; }
        public string? ControllerName { get; set; }
        public string? TextoBotaoAdicionar { get; set; } = "Adicionar";
        public bool Export { get; set; } = false;
        public bool DisabledDetails { get; set; } = false;
        public bool DisabledEdit { get; set; } = false;

    }
}
