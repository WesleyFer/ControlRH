using ControlRH.Areas.Colaborador.Contracts;
using ControlRH.Areas.Colaborador.Models.ViewModels;
using ControlRH.Areas.Colaborador.Models;
using ControlRH.Core.Contracts;
using Microsoft.EntityFrameworkCore;
using ControlRH.Core.Enums;
using System.Net.Sockets;
using System.Net;
using System.Globalization;
using System.Drawing.Printing;
using System.Threading;
using ControlRH.Core.Models;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using ControlRH.Core.Helpers;
using ControlRH.Api.Dtos;
using ControlRH.Areas.Admin.Models.ViewModels;

namespace ControlRH.Areas.Colaborador.Service;

public class PontoEletronicoService : IPontoEletronicoService
{
    private readonly IUnitOfWork _uow;
    private readonly IQueryContext _queryContext;
    private readonly IUsuarioLogado _usuarioLogado;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IExcelAdapter _excelAdapter;

    public PontoEletronicoService(
        IUnitOfWork uow,
        IQueryContext queryContext,
        IUsuarioLogado usuarioLogado,
        IHttpContextAccessor httpContextAccessor,
        IExcelAdapter excelAdapter)
    {
        _uow = uow;
        _queryContext = queryContext;
        _usuarioLogado = usuarioLogado;
        _httpContextAccessor = httpContextAccessor;
        _excelAdapter = excelAdapter;
    }

    public async Task<IEnumerable<PontoEletronico>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _queryContext.QueryPontosEletronicos
            .ToListAsync(cancellationToken);
    }

    public async Task<PontoEletronico?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _queryContext.QueryPontosEletronicos
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<TabelaPontoEletronicoViewModel> ObterTabelaIndexAsync(DateTime? dePeriodo = null, DateTime? atePeriodo = null, int page = 1, int pageSize = 5, CancellationToken cancellationToken = default)
    {
        if (!_usuarioLogado.Autenticado || string.IsNullOrEmpty(_usuarioLogado.Cpf) || string.IsNullOrEmpty(_usuarioLogado.Pis))
        {
            return new TabelaPontoEletronicoViewModel();
        }

        var query = _queryContext.QueryPontosEletronicos
            .Where(c => c.Cpf == _usuarioLogado.Cpf && c.Pis == _usuarioLogado.Pis);

        if (dePeriodo.HasValue)
        {
            query = query.Where(c => c.DataHora.Date >= dePeriodo.Value.Date);
        }

        if (atePeriodo.HasValue)
        {
            query = query.Where(c => c.DataHora.Date <= atePeriodo.Value.Date);
        }

        var totalItems = await query.CountAsync(cancellationToken);

        var registros = await query
            .OrderByDescending(c => c.DataHora)
            .ToListAsync(cancellationToken);

        var registrosAgrupados = registros
            .GroupBy(c => c.DataHora.Date)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var linhas = registrosAgrupados.Select(grupo =>
        {
            var registros = grupo.ToList();

            var entrada = registros.FirstOrDefault(x => x.Marcacao == MarcacaoType.Entrada)?.DataHora;
            var saidaAlmoco = registros.FirstOrDefault(x => x.Marcacao == MarcacaoType.SaidaIntervalo)?.DataHora;
            var retornoAlmoco = registros.FirstOrDefault(x => x.Marcacao == MarcacaoType.RetornoIntervalo)?.DataHora;
            var saida = registros.LastOrDefault(x => x.Marcacao == MarcacaoType.Saida)?.DataHora;

            TimeSpan horasTrabalhadas = TimeSpan.Zero;
            if (entrada.HasValue && saida.HasValue)
            {
                horasTrabalhadas = saida.Value - entrada.Value;

                if (saidaAlmoco.HasValue && retornoAlmoco.HasValue)
                    horasTrabalhadas -= (retornoAlmoco.Value - saidaAlmoco.Value);
            }

            TimeSpan horasEsperadas = TimeSpan.FromHours(8); // Exemplo: 8h por dia
            TimeSpan horasAtraso = horasTrabalhadas < horasEsperadas ? horasEsperadas - horasTrabalhadas : TimeSpan.Zero;

            return new RegistroPontoViewModel
            {
                Dia = grupo.Key,
                DiaSemana = grupo.Key.ToString("dddd", new CultureInfo("pt-BR")),
                Entrada = entrada?.ToString("HH:mm") ?? "-:-",
                SaidaIntervalo = saidaAlmoco?.ToString("HH:mm") ?? "-:-",
                RetornoIntervalo = retornoAlmoco?.ToString("HH:mm") ?? "-:-",
                Saida = saida?.ToString("HH:mm") ?? "-:-",
                HorasTrabalhadas = horasTrabalhadas.ToString(@"hh\:mm"),
                HorasJustificadas = "00:00",
                HorasEmAtraso = horasAtraso.ToString(@"hh\:mm")
            };
        }).ToList();

        var viewModel = new TabelaPontoEletronicoViewModel
        {
            Data = linhas,
            Columns = Columns,
            PageNumber = page,
            PageSize = pageSize,
            TotalItems = totalItems
        };

        return viewModel;
    }

    private Dictionary<string, string> Columns => new()
    {
        { "Dia", "DATA" },
        { "Entrada", "ENTRADA" },
        { "SaidaIntervalo", "SAÍDA INTERVALO" },
        { "RetornoIntervalo", "RETORNO INTERVALO" },
        { "Saida", "SAÍDA" },
        { "HorasTrabalhadas", "HORAS TRABALHADAS" },
        { "HorasEmAtraso", "HORAS EM ATRASO" },
        { "HorasJustificadas", "HORAS JUSTIFICADAS" },
    };

    public async Task<PontoEletronicoViewModel?> DetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entidade = await GetByIdAsync(id, cancellationToken);
        if (entidade is null)
            return null;

        var viewModel = new PontoEletronicoViewModel();
        viewModel.ToViewModel(entidade);
        return viewModel;
    }

    public async Task<bool> MarcarPontoAsync(PontoEletronicoViewModel viewModel, CancellationToken cancellationToken = default)
    {

        if (!_usuarioLogado.Autenticado || string.IsNullOrEmpty(_usuarioLogado.Cpf) || string.IsNullOrEmpty(_usuarioLogado.Pis))
        {
            return false;
        }

        var entidade = viewModel
            .ToModel(_usuarioLogado.Cpf, _usuarioLogado.Pis);

        var (ip, hostName) = await ObterIPHostnameAsync();

        if (!string.IsNullOrEmpty(ip))
            entidade.AdicionarEnderecoIp(ip);

        if (!string.IsNullOrEmpty(hostName))
            entidade.AdicionarHostname(hostName);

        if (!entidade.IsValid)
        {
            return false;
        }

        //var ultimaMarcacao = await UltimaMarcacaoAsync(entidade.Cpf, cancellationToken);

        //if (ultimaMarcacao != null)
        //{
        //    // Não permitir duas entradas seguidas sem uma saída ou intervalo, por exemplo
        //    if (entidade.Marcacao == MarcacaoType.Entrada && ultimaMarcacao.Marcacao == MarcacaoType.Entrada)
        //    {
        //        entidade.AddNotification("TipoMarcacao", "Não é permitido registrar duas entradas consecutivas sem uma saída.");
        //        return false;
        //    }
        //    // Exemplo: Não permitir uma saída de intervalo sem uma entrada prévia
        //    if (entidade.Marcacao == MarcacaoType.SaidaAlmoco && ultimaMarcacao.Marcacao == MarcacaoType.Saida)
        //    {
        //        entidade.AddNotification("TipoMarcacao", "Não é possível sair para intervalo após uma saída final.");
        //        return false;
        //    }
        //    // Você pode adicionar mais regras aqui para garantir a sequência lógica dos pontos
        //}

        var repositorio = _uow.Repository<PontoEletronico>();
        await repositorio.InsertAsync(entidade, cancellationToken);
        var changes = await _uow.CommitAsync(cancellationToken);

        if (changes <= 0)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> MarcarPontoApiAsync(RegistrarPontoRequest request, CancellationToken cancellationToken = default)
    {

        if (!_usuarioLogado.Autenticado || string.IsNullOrEmpty(_usuarioLogado.Cpf) || string.IsNullOrEmpty(_usuarioLogado.Pis))
        {
            return false;
        }

        var entidade = new PontoEletronico(_usuarioLogado.Cpf, _usuarioLogado.Pis, request.DataHora, request.TipoMarcacao);

        var (ip, hostName) = await ObterIPHostnameAsync();

        if (!string.IsNullOrEmpty(ip))
            entidade.AdicionarEnderecoIp(ip);

        if (!string.IsNullOrEmpty(hostName))
            entidade.AdicionarHostname(hostName);

        if (!entidade.IsValid)
        {
            return false;
        }

        //var ultimaMarcacao = await UltimaMarcacaoAsync(entidade.Cpf, cancellationToken);

        //if (ultimaMarcacao != null)
        //{
        //    if (entidade.Marcacao == MarcacaoType.Entrada && ultimaMarcacao.Marcacao == MarcacaoType.Entrada)
        //    {
        //        entidade.AddNotification("TipoMarcacao", "Não é permitido registrar duas entradas consecutivas sem uma saída.");
        //        return false;
        //    }
        //    if (entidade.Marcacao == MarcacaoType.SaidaIntervalo && ultimaMarcacao.Marcacao == MarcacaoType.Saida)
        //    {
        //        entidade.AddNotification("TipoMarcacao", "Não é possível sair para intervalo após uma saída final.");
        //        return false;
        //    }
        //}

        var repositorio = _uow.Repository<PontoEletronico>();
        await repositorio.InsertAsync(entidade, cancellationToken);
        var changes = await _uow.CommitAsync(cancellationToken);

        if (changes <= 0)
        {
            return false;
        }

        return true;
    }

    public async Task UpdateAsync(Guid id, PontoEletronicoViewModel viewModel, CancellationToken cancellationToken = default)
    {
        var entidade = await GetByIdAsync(id, cancellationToken);

        if (entidade is null)
            return;

        var repositorio = _uow.Repository<PontoEletronico>();

        await repositorio.UpdateAsync(entidade, cancellationToken);

        var changes = await _uow.CommitAsync(cancellationToken);

        if (changes <= 0)
        {
            entidade.AddNotification("", "Erro ao atualizar.");
        }
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entidade = await GetByIdAsync(id, cancellationToken);
        if (entidade is null)
            return;

        var repositorio = _uow.Repository<PontoEletronico>();
        await repositorio.DeleteAsync(entidade, cancellationToken);
        var changes = await _uow.CommitAsync(cancellationToken);

        if (changes <= 0)
        {
            entidade.AddNotification("", "Erro ao deletar.");
            return;
        }
    }

    public async Task<PontoEletronico?> UltimaMarcacaoAsync(string cpf, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(cpf))
            return null;

        return await _queryContext.QueryPontosEletronicos
            .Where(c => c.Cpf == cpf)
            .OrderByDescending(c => c.DataHora)
            .FirstOrDefaultAsync(cancellationToken);
    }

    private async Task<(string? enderecoIP, string? hostName)> ObterIPHostnameAsync()
    {
        string ipAddress = string.Empty;
        string hostname = string.Empty;

        if (_httpContextAccessor.HttpContext != null)
        {
            ipAddress = _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
            }
            if (!string.IsNullOrEmpty(ipAddress) && ipAddress.Contains(','))
            {
                ipAddress = ipAddress.Split(',').FirstOrDefault()?.Trim();
            }

            if (!string.IsNullOrEmpty(ipAddress))
            {
                try
                {
                    if (IPAddress.TryParse(ipAddress, out IPAddress clientIp))
                    {
                        IPHostEntry hostEntry = await Dns.GetHostEntryAsync(clientIp);
                        hostname = hostEntry.HostName;
                    }
                }
                catch (SocketException)
                {
                    hostname = "Não resolvido";
                }
            }
        }

        return (ipAddress, hostname);
    }

    public async Task<byte[]?> ObterEspelhoPontoAsync(CancellationToken cancellationToken = default)
    {
        if (!_usuarioLogado.Autenticado || string.IsNullOrEmpty(_usuarioLogado.Cpf) || string.IsNullOrEmpty(_usuarioLogado.Pis))
        {
            return Array.Empty<byte>();
        }

        var query = _queryContext.QueryPontosEletronicos
            .Where(c => c.Cpf == _usuarioLogado.Cpf && c.Pis == _usuarioLogado.Pis);

        var totalItems = await query.CountAsync(cancellationToken);

        var registros = await query
            .OrderByDescending(c => c.DataHora)
            .ToListAsync(cancellationToken);

        var registrosAgrupados = registros
            .GroupBy(c => c.DataHora.Date)
            .ToList();

        var espelhoPonto = registrosAgrupados.Select(grupo =>
        {
            var registros = grupo.ToList();

            var entrada = registros.FirstOrDefault(x => x.Marcacao == MarcacaoType.Entrada)?.DataHora;
            var saidaAlmoco = registros.FirstOrDefault(x => x.Marcacao == MarcacaoType.SaidaIntervalo)?.DataHora;
            var retornoAlmoco = registros.FirstOrDefault(x => x.Marcacao == MarcacaoType.RetornoIntervalo)?.DataHora;
            var saida = registros.LastOrDefault(x => x.Marcacao == MarcacaoType.Saida)?.DataHora;

            TimeSpan horasTrabalhadas = TimeSpan.Zero;
            if (entrada.HasValue && saida.HasValue)
            {
                horasTrabalhadas = saida.Value - entrada.Value;

                if (saidaAlmoco.HasValue && retornoAlmoco.HasValue)
                    horasTrabalhadas -= (retornoAlmoco.Value - saidaAlmoco.Value);
            }

            TimeSpan horasEsperadas = TimeSpan.FromHours(8); // Exemplo: 8h por dia
            TimeSpan horasAtraso = horasTrabalhadas < horasEsperadas ? horasEsperadas - horasTrabalhadas : TimeSpan.Zero;

            return new RegistroPontoViewModel
            {
                Dia = grupo.Key,
                DiaSemana = grupo.Key.ToString("dddd", new CultureInfo("pt-BR")),
                Entrada = entrada?.ToString("HH:mm") ?? "-:-",
                SaidaIntervalo = saidaAlmoco?.ToString("HH:mm") ?? "-:-",
                RetornoIntervalo = retornoAlmoco?.ToString("HH:mm") ?? "-:-",
                Saida = saida?.ToString("HH:mm") ?? "-:-",
                HorasTrabalhadas = horasTrabalhadas.ToString(@"hh\:mm"),
                HorasJustificadas = "00:00",
                HorasEmAtraso = horasAtraso.ToString(@"hh\:mm")
            };
        }).ToList();


        var cabecalho = new Dictionary<string, object>
        {
            {"Nome", _usuarioLogado.Nome.ToUpper() },
            {"CPF",  Utils.FormatarCpf(_usuarioLogado.Cpf)},
            {"PIS",  Utils.FormatarPis(_usuarioLogado.Pis)},
        };

        var colunas = new List<ExcelColunaInfo>
        {
            new ExcelColunaInfo{ NomeColuna = "DATA", Propriedade = "Dia", FormatoExcel = "dd/MM/yyyy"},
            new ExcelColunaInfo{ NomeColuna = "ENTRADA", Propriedade = "Entrada"},
            new ExcelColunaInfo{ NomeColuna = "SAÍDA INTERVALO", Propriedade = "SaidaIntervalo"},
            new ExcelColunaInfo{ NomeColuna = "RETORNO INTERVALO", Propriedade = "RetornoIntervalo"},
            new ExcelColunaInfo{ NomeColuna = "SAÍDA", Propriedade = "Saida"},
            new ExcelColunaInfo{ NomeColuna = "HORAS TRABALHADAS", Propriedade = "HorasTrabalhadas"},
            new ExcelColunaInfo{ NomeColuna = "HORAS EM ATRASO", Propriedade = "HorasEmAtraso"},
            new ExcelColunaInfo{ NomeColuna = "HORAS JUSTIFICADAS", Propriedade = "HorasJustificadas"},
        };

        return _excelAdapter.GerarExcel("Espelho de Ponto", "ESPELHO DE PONTO", cabecalho, colunas, espelhoPonto);
    }

    public async Task<DocumentoDownloadViewModel?> DownloadDocumentoAsync(Guid id, CancellationToken cancellationToken = default)
    {

        if (!_usuarioLogado.Autenticado ||
            string.IsNullOrEmpty(_usuarioLogado.Cpf) ||
            string.IsNullOrEmpty(_usuarioLogado.Pis) ||
            _usuarioLogado.ColaboradorId == Guid.Empty ||
            _usuarioLogado.CarteiraClienteId == Guid.Empty)
        {
            return null;
        }

        var documento = await _queryContext.QueryDocumentos
            .Where(c => c.Id == id
                && c.CarteiraClienteId == _usuarioLogado.CarteiraClienteId
                && (c.ColaboradorId == null || c.ColaboradorId == _usuarioLogado.ColaboradorId))
            .Select(c => new { c.CaminhoArquivo, c.Nome })
            .FirstOrDefaultAsync(cancellationToken);

        if (documento == null)
            return null;

        if (!File.Exists(documento.CaminhoArquivo))
            return null;

        return new DocumentoDownloadViewModel
        {
            Conteudo = await File.ReadAllBytesAsync(documento.CaminhoArquivo, cancellationToken),
            Nome = documento.Nome
        };
    }

    public async Task<TabelaDocumentoViewModel> ObterTodosDocumentosAsync(int page = 1, int pageSize = 5, CancellationToken cancellationToken = default)
    {
        if (!_usuarioLogado.Autenticado ||
            string.IsNullOrEmpty(_usuarioLogado.Cpf) ||
            string.IsNullOrEmpty(_usuarioLogado.Pis) ||
            _usuarioLogado.ColaboradorId == Guid.Empty ||
            _usuarioLogado.CarteiraClienteId == Guid.Empty)
        {
            return new TabelaDocumentoViewModel();
        }

        // Pega documentos da carteira que são compartilhados (ColaboradorId nulo)
        var query = _queryContext.QueryDocumentos
            .Where(c => c.CarteiraClienteId == _usuarioLogado.CarteiraClienteId && c.ColaboradorId == null);

        // Se o usuário logado tiver ColaboradorId válido, adiciona documentos específicos dele
        if (_usuarioLogado.ColaboradorId != Guid.Empty)
        {
            query = query
                .Union(_queryContext.QueryDocumentos
                    .Where(c => c.CarteiraClienteId == _usuarioLogado.CarteiraClienteId
                             && c.ColaboradorId == _usuarioLogado.ColaboradorId));
        }

        var totalItems = await query.CountAsync(cancellationToken);

        var registros = await query
            .OrderByDescending(c => c.DataCriacao)
            .Select(c => new DocumentoPontoViewModel
            {
                Id = c.Id,
                Nome = c.Nome,
            })
            .ToListAsync(cancellationToken);

        var viewModel = new TabelaDocumentoViewModel
        {
            Data = registros,
            Columns = DocumentColumns,
            PageNumber = page,
            PageSize = pageSize,
            TotalItems = totalItems
        };

        return viewModel;
    }

    private Dictionary<string, string> DocumentColumns => new()
    {
        { "Nome", "DOCUMENTO" },
    };
}
