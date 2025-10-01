using ControlRH;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// Configurar Kestrel com certificado HTTPS (apenas quando estiver rodando no container)
builder.WebHost.ConfigureKestrel(options =>
{
    var certPath = "/https/localhost.pfx"; // caminho dentro do container
    var certPassword = "123456";   // coloque a senha do pfx

    if (File.Exists(certPath))
    {
        var cert = new X509Certificate2(
            certPath,
            certPassword,
            X509KeyStorageFlags.EphemeralKeySet | X509KeyStorageFlags.MachineKeySet
        );

        options.ListenAnyIP(5026); // HTTP
        options.ListenAnyIP(7158, listenOptions =>
        {
            listenOptions.UseHttps(cert);
        });
    }
    else
    {
        // fallback: só HTTP (evita falha se o cert não for encontrado)
        options.ListenAnyIP(5026);
    }
});

builder.Host.ConfigureServices((context, services) =>
{
    var startup = new Startup(context.Configuration);
    startup.ConfigureServices(services);
});

var app = builder.Build();

var env = app.Services.GetRequiredService<IWebHostEnvironment>();
var startup = new Startup(app.Configuration);
startup.Configure(app, env);

app.Run();
