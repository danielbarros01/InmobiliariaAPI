using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;

namespace InmobiliariaV2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles); ////Esto indica que se debe ignorar cualquier referencia circular o relación cíclica durante la serialización JSON

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddDbContext<DataContext>(
                options => options.UseMySql(
                    Configuration["ConnectionStrings:MySql"],
                    ServerVersion.AutoDetect(Configuration["ConnectionStrings:MySql"])
                ));



            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    // Especifica los parámetros de validación del token.
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        //Estas propiedades controlan qué aspectos del token se validarán.
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        //
                        ValidIssuer = Configuration["TokenAuthentication:Issuer"], //Especifica el emisor válido esperado del token
                        ValidAudience = Configuration["TokenAuthentication:Audience"], // Especifica la audiencia válida esperada del token
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(
                            Configuration["TokenAuthentication:SecretKey"])), //Especifica la clave de firma utilizada para verificar la autenticidad del token. 
                    };

                    // opción extra para usar el token en el hub y otras peticiones sin encabezado (enlaces, src de img, etc.)
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            // Leer el token desde el query string
                            var accessToken = context.Request.Query["access_token"];
                            // Si el request es para el Hub u otra ruta seleccionada...
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/chatsegurohub") ||
                                path.StartsWithSegments("/api/propietarios/reset") ||
                                path.StartsWithSegments("/api/propietarios/token")))
                            {//reemplazar las urls por las necesarias ruta ⬆
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
        }

        internal void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection(); //redirige el tráfico HTTP a HTTPS

            /* CORS */
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            /*  */

            app.UseRouting(); //establece el enrutamiento de la solicitud

            app.UseAuthentication();
            app.UseAuthorization(); //agrega el middleware de autorización

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }


    }
}
