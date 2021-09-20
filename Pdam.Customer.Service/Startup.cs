using System;
using System.Linq;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using Pdam.Common.Shared.Fault;
using Pdam.Common.Shared.Helper;
using Pdam.Common.Shared.Infrastructure;
using Pdam.Common.Shared.Logging;
using Pdam.Customer.Service.DataContext;
using Pdam.Customer.Service.Infrastructures;

namespace Pdam.Customer.Service
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddOData(options => options
                        .Select().Filter().Count().OrderBy().Expand().EnableQueryFeatures()
                        .SetMaxTop(1000) // enable usage of $top
                        .AddRouteComponents("query", GetEdmModel()) // enable OData routing
                )
                .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .AddDateTimeFormat();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IApiLogger, ApiLogger>();
            services.AddDbContext<CustomerContext>(c =>
                c.UseNpgsql(Environment.GetEnvironmentVariable("PdamCustomerConnectionString") ?? string.Empty));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionDecorator<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationDecorator<,>));
            
            services.Scan(scan => scan.FromEntryAssembly()
                .AddClasses(classes => classes.AssignableTo(typeof(IRequestValidator<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );
            
            services.AddMediatR(typeof(Features.Customers.Handler));
            services.AddAutoMapper(typeof(CustomerProfile));
            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(type => type.ToString());
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Pdam.Customer.Service", Version = "v1"});
            });
            services.AddHealthChecks()
                .AddNpgSql(Environment.GetEnvironmentVariable("PdamCustomerConnectionString"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.MigrateDatabase();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pdam.Customer.Service v1"));
                app.SetupInitData();
            }
            app.UseMiddleware<FailureMiddleware>();

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health"/*, new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                }*/);

                /*endpoints.MapHealthChecksUI(setup =>
                {
                    setup.UIPath = "/health-ui";
                    setup.ApiPath = "/health-json";  
                });*/
            });
        }

        private static IEdmModel GetEdmModel()
        {
            var modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EntitySet<DataContext.Customer>("Customer"); // must match BooksController
            modelBuilder.EntitySet<CustomerAddress>("Address"); 
            modelBuilder.EntitySet<CustomerAsset>("Asset"); 
            modelBuilder.EntitySet<CustomerContact>("Contact"); 
            modelBuilder.EntitySet<CustomerStatusLog>("StatusLog"); 
            
            return modelBuilder.GetEdmModel();
        }
    }
}