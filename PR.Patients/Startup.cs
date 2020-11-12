using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using PR.Patients.Model;
using PR.Patients.Services;

namespace PR.Patients
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
            services.AddControllers(options =>
                  {
                      options.RespectBrowserAcceptHeader = true;
                  }).AddXmlSerializerFormatters();
 

            services.AddControllers();

            services.AddScoped<ServiceBusSender>();

            services.AddDbContext<PatientsDataContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionString"));
                });


            //#3 >>
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
                { options.Authority = "https://login.microsoftonline.com/146ab906-a33d-47df-ae47-fb16c039ef96/v2.0/";
                    options.Audience = "api://67dd9cfb-4344-4cc8-a2ca-573f6bb4422f";
                    options.TokenValidationParameters.ValidateIssuer = false; //tu mo¿na dodaæ dozwolonych dostawców
                    options.IncludeErrorDetails = true;
                    
                }
             );

            IdentityModelEventSource.ShowPII = true;
            //#3 <<

            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //#3
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("swagger/v1/swagger.json", "PR.Patients description");
            }

            );
        }
    }
}
