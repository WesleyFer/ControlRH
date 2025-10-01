using ControlRH.Areas.Admin.Contracts;
using ControlRH.Areas.Admin.Services;
using ControlRH.Areas.Colaborador.Contracts;
using ControlRH.Areas.Colaborador.Service;
using ControlRH.Core.Adapters;
using ControlRH.Core.Configurations;
using ControlRH.Core.Contracts;
using ControlRH.Core.Models;
using ControlRH.Data;
using ControlRH.Data.Repositories;
using ControlRH.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

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
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllCredentials", builder =>
                {
                    builder
                        .SetIsOriginAllowed(_ => true) // aceita qualquer origem
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.LoginPath = "/Auth/Login";
                    options.AccessDeniedPath = "/Auth/AcessoNegado";
                    options.ExpireTimeSpan = TimeSpan.FromHours(8);
                    options.SlidingExpiration = true;
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.None;
                    options.Cookie.SameSite = SameSiteMode.Strict;

                    options.Events = new CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = context =>
                        {
                            if (context.Request.Path.StartsWithSegments("/api"))
                            {
                                context.Response.StatusCode = 401;
                                return Task.CompletedTask;
                            }

                            context.Response.Redirect(context.RedirectUri);
                            return Task.CompletedTask;
                        },
                        OnRedirectToAccessDenied = context =>
                        {
                            context.Response.Redirect("/Auth/AcessoNegado");
                            return Task.CompletedTask;
                        }
                    };
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration["Jwt:Secret"])
                        )
                    };
                });

            services.AddAuthorization();

            services.AddHttpContextAccessor();
            services.AddScoped<IUsuarioLogado, UsuarioLogado>();

            var mySqlConnectionString = Configuration.GetConnectionString("MySql");
            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseMySql(
                    mySqlConnectionString,
                    ServerVersion.AutoDetect(mySqlConnectionString),
                    mySqlOptions =>
                    {
                        mySqlOptions.CommandTimeout(360);
                        mySqlOptions.EnableIndexOptimizedBooleanColumns();
                        mySqlOptions.UseRelationalNulls();
                        mySqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    }
                );
            }, ServiceLifetime.Scoped, ServiceLifetime.Scoped);

            services.AddScoped<DbContext>(provider => provider.GetRequiredService<ApplicationContext>());

            services.AddScoped<IQueryContext, ApplicationContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork<ApplicationContext>>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IExcelAdapter, ExcelAdapter>();

            services.Configure<UploadSettings>(Configuration.GetSection("UploadSettings"));

            services.AddScoped<IJornadaTrabalhoService, JornadaTrabalhoService>();
            services.AddScoped<ICarteiraClienteService, CarteiraClienteService>();
            services.AddScoped<IColaboradorService, ColaboradorService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IGrupoService, GrupoService>();
            services.AddScoped<ICargoService, CargoService>();
            services.AddScoped<IDocumentoService, DocumentoService>();

            services.AddMemoryCache();
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IPontoEletronicoService, PontoEletronicoService>();

            services.AddControllersWithViews();

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Control RH API", Version = "v1" });

                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var actionApiVersionModel = apiDesc.ActionDescriptor.EndpointMetadata
                        .OfType<ApiControllerAttribute>()
                        .FirstOrDefault();

                    return actionApiVersionModel != null;
                });

                c.TagActionsBy(apiDesc =>
                {
                    return new[] { apiDesc.RelativePath.Split('/')[0] };
                });
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Control RH API V1");
            });

            app.UseCors("AllowAllCredentials");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // Rota Api
                endpoints.MapControllers();

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
