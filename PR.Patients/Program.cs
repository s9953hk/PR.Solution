using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace PR.Patients
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {

                    //#4>> 
                    //NuGet Package Serilog.AspNetCore
                    //Serilog.Sinks.ApplicationInsights
                    webBuilder.UseSerilog((context, loggerConfig) =>
                    {
                        var telemetryConfig = TelemetryConfiguration.CreateDefault();
                        telemetryConfig.InstrumentationKey = context.Configuration["ApplicationInsights:InstrumentationKey"];

                        loggerConfig.ReadFrom.Configuration(context.Configuration);
                        loggerConfig.WriteTo.ApplicationInsights(telemetryConfig, TelemetryConverter.Traces);
                        if (context.HostingEnvironment.IsDevelopment())
                        {
                            loggerConfig.WriteTo.Console();
                        }
                        //loggerConfig.WriteTo.File("log_.txt", rollingInterval: RollingInterval.Day);
                    });
                    //#4<<
                    webBuilder.UseStartup<Startup>();
                });
    }
}
