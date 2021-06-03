using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using PlayList.DataAccess;
using PlayList.Service;

namespace PlayList.Managerment.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services,IWebHostEnvironment env)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PlayList.Managerment.WebApi", Version = "v1" });
            });
            // add authenticate to DI
            services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateAudience = true,
                    RequireExpirationTime = true,
                    ClockSkew = new TimeSpan(0, 10, 0)
                };
                options.Authority = "https://localhost:7001";
                options.Audience = "Managerment";
                options.RequireHttpsMetadata = false;
            });

            MySqlServerVersion dataBaseVersion = new MySqlServerVersion(new Version(8, 0, 25));
            if (env.IsDevelopment())
            {
                services.AddDbContext<PlayListDbContext>(options =>
                {
                    options.UseMySql(Configuration.GetConnectionString("DevMysqlConnectStr"),
                    dataBaseVersion);
                });
            }
            else
            {
                services.AddDbContext<PlayListDbContext>(options =>
                {
                    options.UseMySql(Configuration.GetConnectionString("MysqlConnectStr"),
                    dataBaseVersion);
                });
            }

            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IManagerService, ManagerService>();
            services.AddScoped<IVoteService, VoteService>();
            services.AddScoped<IPlayInfoService, PlayInfoService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PlayList.Managerment.WebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
