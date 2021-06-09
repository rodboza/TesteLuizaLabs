using Favoritos.Domain.Handlers;
using Favoritos.Domain.Repositories;
using Favoritos.Infra.Contexts;
using Favoritos.Infra.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Favoritos.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();
            services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("Database"));

            services.AddTransient<IClienteRepository, ClienteRepository>();
            services.AddTransient<IFavoritoRepository, FavoritoRepository>();
            services.AddTransient<ILoginRepository, LoginRepository>();
            services.AddTransient<FavoritoHandler, FavoritoHandler>();
            services.AddTransient<FavoritoHandler, FavoritoHandler>();
            services.AddTransient<LoginHandler, LoginHandler>();
            services.AddTransient<CargaInicialHandler, CargaInicialHandler>();

            var key = Encoding.ASCII.GetBytes(LoginRepository.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var loginRepository = context.HttpContext.RequestServices.GetRequiredService<ILoginRepository>();
                        var userId = Guid.Parse(context.Principal.Identity.Name);
                        var user = loginRepository.GetById(userId);
                        if (user == null)
                        {
                            // return unauthorized if user no longer exists
                            context.Fail("Unauthorized");
                            //Task<IActionResult> saida = (Task<IActionResult>)GenericCommandResult.UnauthorizedResult("Acesso não autorizado", user);
                            //return saida;
                        }

                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
