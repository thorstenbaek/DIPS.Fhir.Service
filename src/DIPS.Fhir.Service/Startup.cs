using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NHibernate.NetCore;
using DIPS.Fhir.Service.Entities;
using System;
using DIPS.Fhir.Service.Transformers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using DIPS.Fhir.Service.Configuration;

namespace DIPS.Fhir.Service
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
            var configuration = new NHibernate.Cfg.Configuration();
            configuration.DataBaseIntegration(c =>
            {
                c.Driver<NHibernate.Driver.NpgsqlDriver>();
                c.Dialect<NHibernate.Dialect.PostgreSQL83Dialect>();

                var connectionString = System.Environment.GetEnvironmentVariable("CONNECTION_STRING");
                Console.WriteLine("Given ConnectionString:" + connectionString);
                c.ConnectionString = connectionString;
                c.LogFormattedSql = true;
                c.LogSqlInConsole = true;
            });

            var mapper = new NHibernate.Mapping.ByCode.ModelMapper();
            mapper.AddMapping<PatientMapping>();
            mapper.AddMapping<PractitionerMapping>();
            mapper.AddMapping<EncounterMapping>();
            mapper.AddMapping<ObservationMapping>();
            mapper.AddMapping<DocumentReferenceMapping>();

            var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
            configuration.AddMapping(mapping);

            services.AddSingleton<IEnvironment, EnvironmentImpl>();
            services.AddSingleton<IConfigurationLoader, ConfigurationLoader>();
            services.AddSingleton<ICentralConfiguration, CentralConfiguration>();
            services.AddSingleton<IObservationTransformer, ObservationTransformer>();

            // add NHibernate services;
            services.AddHibernate(configuration);

            services.AddControllers();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    "v1", 
                    new OpenApiInfo 
                    { 
                        Title = "DIPS FHIR API", 
                        Version = "v1" 
                    });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .SetIsOriginAllowed((host) => true)
                        .AllowAnyHeader());
            });            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                c.RoutePrefix = string.Empty;
            });

            // Use loggerFactory as NHibernate logger factory.
            loggerFactory.UseAsHibernateLoggerFactory();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
