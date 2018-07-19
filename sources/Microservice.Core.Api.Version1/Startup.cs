using System;
using System.Text;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microservice.Core.Api.Version1.IoC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NLog.Extensions.Logging;
using StructureMap;
using Swashbuckle.AspNetCore.Swagger;

namespace Microservice.Core.Api.Version1
{
    public class Startup : MSStartup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("globalsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"globalsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

            builder.AddEnvironmentVariables();
            this.Configuration = builder.Build();

            this.HostingEnvironment = env;
        }

        public IConfigurationRoot Configuration { get; }

        public IHostingEnvironment HostingEnvironment { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services
                .AddMvc(MvcServiceSetup).
                AddJsonOptions(options =>
                {
                    options.SerializerSettings.Formatting = Formatting.Indented;
                });

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Audience = Configuration[AppConfigConsts.TokenAudience];
                options.ClaimsIssuer = Configuration[AppConfigConsts.TokenIssuer];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = Configuration[AppConfigConsts.TokenIssuer],
                    ValidAudience = Configuration[AppConfigConsts.TokenAudience],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Configuration[AppConfigConsts.TokenKey]))
                };
            });

            services.AddApiVersioning(o => o.ReportApiVersions = true);

            services.AddMvc().AddFluentValidation();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "Microservice", Version = "v1" });
                options.CustomSchemaIds(x => x.FullName);
                options.DescribeAllEnumsAsStrings();
                options.AddSecurityDefinition("Authentication", new ApiKeyScheme { Description = "Authorization Token", Name = "Authorization", In = "header", Type = "apiKey" });

                this.SwaggerServiceSetup(options);
            });

            services.AddAutoMapper(this.AutoMapperServiceSetup);

            var dependencyContainer = new ApiContainer(Configuration, this.HostingEnvironment.EnvironmentName);

            dependencyContainer.Container.Populate(services);

            return dependencyContainer.Container.GetInstance<IServiceProvider>();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();

            app.UseAuthentication();

            app.UseMvc();

            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
            });

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(Configuration[AppConfigConsts.SwaggerPath], "Microservice API");
            });
        }
    }
}
