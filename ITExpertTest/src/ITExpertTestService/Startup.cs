using ITExpertTest.Business.Commands;
using ITExpertTest.Business.Commands.Interfaces;
using ITExpertTest.Business.Interfaces;
using ITExpertTestService.Data.Providers.Interfaces;
using ITExpertTestService.Data.Repository;
using ITExpertTestService.Data.Repository.Interfaces;
using ITExpertTestService.Mappers;
using ITExpertTestService.Mappers.Interfaces;
using ITExpertTestService.Provider.Ef.MsSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace ITExpertTest
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
            services.AddCors(options =>
            {
                options.AddPolicy(
                    "ITExpert",
                    builder =>
                    {
                        builder
                            .WithOrigins(
                            //    "https://*.ltdo.xyz",
                            //    "http://*.ltdo.xyz",
                            //    "http://ltdo.xyz",
                            //    "http://ltdo.xyz:9802",
                            //    "http://localhost:4200",
                            //    "http://localhost:4500")
                            "http://localhost:3000")
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            services.AddControllers();
            services.AddHttpContextAccessor();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ITExpertTest", Version = "v1" });
            });

            string connStr = Environment.GetEnvironmentVariable("ConnectionString");
            if (string.IsNullOrEmpty(connStr))
            {
                connStr = Configuration.GetConnectionString("SQLConnectionString");
            }
            else
            {
            }
            services.AddScoped<IDataProvider, ITExpertServiceDbContext>();
            services.AddTransient<IObjectRepository, ObjectRepository>();
            services.AddTransient<IMapperCreateObjectRequest, MapperObjectRequest>();
            services.AddTransient<IMapperFindObjectResponse, MapperObjectResponse>();
            services.AddTransient<ICreateObjectsCommand, CreateObjectsCommand>();
            services.AddTransient<IFindObjectsCommand, FindObjectsCommand>();

            services.AddDbContext<ITExpertServiceDbContext>(options =>
            {
                options.UseSqlServer(connStr);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            UpdateDatabase(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ITExpertTest v1"));
            }

            app.UseCors("ITExpert");
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public static void UpdateDatabase(IApplicationBuilder app)
        {
            using IServiceScope serviceScope = app.ApplicationServices
              .GetRequiredService<IServiceScopeFactory>()
              .CreateScope();

            using ITExpertServiceDbContext context = serviceScope.ServiceProvider.GetService<ITExpertServiceDbContext>();

            context.Database.Migrate();
        }
    }
}
