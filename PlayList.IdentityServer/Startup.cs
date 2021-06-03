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
using PlayList.IdentityServer.Validator;
using PlayList.Service;
using Pomelo.EntityFrameworkCore.MySql;

namespace PlayList.IdentityServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services, IWebHostEnvironment env)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PlayList.IdentityServer", Version = "v1" });
            });
            var builder = services.AddIdentityServer()
            .AddDeveloperSigningCredential()//添加一个开发用的秘钥文件,只要不是手动删除,长期有效
            .AddInMemoryApiResources(Configs.ApiResources)
            .AddInMemoryApiScopes(Configs.ApiScopes)
            .AddInMemoryClients(Configs.Clients);
            // add a redis to store the token
            //.AddOperationalStore();

            builder.AddProfileService<DefaultProfileService>();
            builder.AddResourceOwnerValidator<ManagerPasswordValidator>();
            //builder.AddResourceOwnerValidator<CutomerPasswordValidator>();
            MySqlServerVersion dataBaseVersion = new MySqlServerVersion(new Version(8, 0, 25));

            if (env.IsDevelopment())
            { 
                services.AddDbContext<PlayListDbContext>(options =>
                {
                    options.UseMySql(Configuration.GetConnectionString("MysqlConnectStr"),
                    dataBaseVersion,
                    b => b.MigrationsAssembly("PlayList.IdentityServer"));
                });
            }
            else
            {
                 services.AddDbContext<PlayListDbContext>(options =>
                {
                    options.UseMySql(Configuration.GetConnectionString("DevMysqlConnectStr"),
                    dataBaseVersion,
                    b => b.MigrationsAssembly("PlayList.IdentityServer"));
                });
            }


            // 分别添加不同的Token的验证
            // builder.AddExtensionGrantValidator<ManagerValidator>();
            // builder.AddExtensionGrantValidator<CustomerValidator>();

            // add DI Container
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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PlayList.IdentityServer v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // app.UseAuthentication();

            // app.UseAuthorization();

            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }


}
