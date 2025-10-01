using ControlRH;

var builder = WebApplication.CreateBuilder(args);

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
