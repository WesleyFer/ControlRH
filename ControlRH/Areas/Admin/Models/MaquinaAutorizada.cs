using ControlRH.Core.Models;

namespace ControlRH.Areas.Admin.Models;

public class MaquinaAutorizada : Entidade
{
    protected MaquinaAutorizada() { }

    public string Nome { get; private set; }
    
    public string EnderecoIp { get; private set; }
    
    public string Hostname { get; private set; }
    
    public string MacAddress { get; private set; }
    
    public bool Ativo { get; private set; }
    
    public string LocalFisico { get; private set; }

    public string ResponsavelCadastro { get; private set; }
}
