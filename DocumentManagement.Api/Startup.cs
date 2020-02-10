using Autofac;
using DocumentManagement.Api.Infrastructure.AppSettings;
using DocumentManagement.Api.Infrastructure.AutofacModules;
using DocumentManagement.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace DocumentManagement.Api
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Document Management API", Version = "v1" });
            });

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddDbContext<DocumentDbContext>(options =>
            {
                var documentDbConfig = Configuration.GetSection(
                    "DocumentDb").Get<DocumentDbSettings>();

                options.UseCosmos(documentDbConfig.Endpoint, documentDbConfig.Key,
                    databaseName: documentDbConfig.DbName);
            });

            services.AddControllers();

            //services.AddScoped<ExceptionCompensation>();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new ApplicationModule(
                Configuration.GetSection("UploadFileSettings").Get<UploadFileSettings>()));
            builder.RegisterModule(new MediatorModule());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Document Management API");
                });
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
