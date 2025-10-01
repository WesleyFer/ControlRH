using ControlRH.Core.Enums;

namespace ControlRH.Api.Dtos
{
    public class RegistrarPontoRequest
    {
        public DateTime DataHora { get; set; }

        public MarcacaoType TipoMarcacao { get; set; }

    }
}
