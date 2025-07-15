using ControlRH.Areas.Admin.Contracts;
using ControlRH.Areas.Admin.Services;
using ControlRH.Core.Adapters;
using ControlRH.Core.Application;
using ControlRH.Core.Contracts;
using ControlRH.Core.Models;
using ControlRH.Data;
using ControlRH.Data.Repositories;
using ControlRH.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ControlRH
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var keycloakSection = Configuration.GetSection("Authentication:Keycloak");
            services.Configure<KeycloakOptions>(keycloakSection);

            var keycloak = keycloakSection.Get<KeycloakOptions>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Conta/Login";
                    options.AccessDeniedPath = "/Conta/AcessoNegado";
                    options.ExpireTimeSpan = TimeSpan.FromHours(3);
                    options.SlidingExpiration = true;

                    options.Events = new CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = contexto =>
                        {
                            if (contexto.Request.Path.StartsWithSegments("/api"))
                            {
                                contexto.Response.StatusCode = 401;
                                return Task.CompletedTask;
                            }

                            contexto.Response.Redirect(contexto.RedirectUri);
                            return Task.CompletedTask;
                        }
                    };
                })
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = keycloak?.Authority;
                    options.ClientId = keycloak?.ClientId;
                    options.ClientSecret = keycloak?.ClientSecret;
                    options.ResponseType = "code";

                    options.SaveTokens = true;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "preferred_username",
                        RoleClaimType = "roles"
                    };

                    options.RequireHttpsMetadata = false;  // só em dev

                    options.Events = new OpenIdConnectEvents
                    {
                        OnRedirectToIdentityProviderForSignOut = async context =>
                        {
                            var authenticateResult = await context.HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                            var idToken = authenticateResult?.Properties?.GetTokenValue("id_token");

                            var logoutUri = $"{keycloak?.Authority}/protocol/openid-connect/logout" +
                                            $"?id_token_hint={idToken}" +
                                            $"&post_logout_redirect_uri={Uri.EscapeDataString(keycloak?.PostLogoutRedirectUri)}";

                            context.Response.Redirect(logoutUri);
                            context.HandleResponse();
                        }
                    };
                });

            services.AddHttpContextAccessor();
            services.AddScoped<IUsuarioLogado, UsuarioLogado>();

            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SqlServer"));
            });

            services.AddScoped<DbContext>(provider => provider.GetRequiredService<ApplicationContext>());

            services.AddScoped<IQueryContext, ApplicationContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork<ApplicationContext>>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IExcelAdapter, ExcelAdapter>();

            //services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
            services.AddScoped<IJornadaTrabalhoService, JornadaTrabalhoService>();
            services.AddScoped<ICarteiraClienteService, CarteiraClienteService>();
            services.AddScoped<IColaboradorService, ColaboradorService>();
            services.AddScoped<IPontoEletronicoService, PontoEletronicoService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IGrupoService, GrupoService>();

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // Rota para Áreas
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                // Rota padrão para controllers fora das áreas
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
