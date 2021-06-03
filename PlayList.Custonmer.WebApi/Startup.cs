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
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PlayList.DataAccess;
using PlayList.Service;

namespace PlayList.Custonmer.WebApi
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PlayList.Custonmer.WebApi", Version = "v1" });
            });

            // add token validate
            services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
            {
                options.Audience = "Customer";
                options.Authority = "https://localhost:7001/";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true
                };

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



                //options.RequireHttpsMetadata=false;
                services.AddScoped<ICustomerService, CustomerService>();
                //services.AddScoped<IManagerService, ManagerService>();
                services.AddScoped<IVoteService, VoteService>();
                services.AddScoped<IPlayInfoService, PlayInfoService>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PlayList.Custonmer.WebApi v1"));
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
