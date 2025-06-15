using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using ValidacionDNI_Backend.Extensions;
using ValidacionDNI_Backend.Models;
using ValidacionDNI_Backend.cronjob;
using System;
using NLog.Web;
using Microsoft.OpenApi.Models;

namespace ValidacionDNI_Backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Servicios personalizados
            services.ConfigureCors();
            services.ConfigureSwagger();
            services.ConfigureJWT(Configuration);

            // Configuraci�n propia
            services.AddOptions();
            services.Configure<beMySettings>(Configuration.GetSection("MySettings"));

            // Cron job
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<IWebHostEnvironment>(Environment);
            services.AddCronJob<MySchedulerJob>(options =>
            {
                options.CronExpression = "00 00 * * *"; // Corre a medianoche
                options.TimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Validacion DNI API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
            logger.Debug("Iniciando App");

            app.UseCors("CorsPolicy");

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Validacion DNI IA");
                c.RoutePrefix = string.Empty; // Para que Swagger est� en "/"
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Validacion DNI API v1");
                c.RoutePrefix = string.Empty; // Esto hace que Swagger est� en la ra�z: http://localhost:5157/
            });
            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
