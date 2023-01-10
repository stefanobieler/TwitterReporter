// See https://aka.ms/new-console-template for more information

using TwitterReporter.ConsoleApp.Options;
using TwitterReporter.ConsoleApp.Services;
using TwitterReporter.ConsoleApp.Services.HttpServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using TwitterReporter.ConsoleApp.Services.Abstractions;

namespace TwitterReporter.ConsoleApp;

internal class Program
{
    public static void Main(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(BuildConfig)
            .ConfigureServices((context, services) =>
            {
                services.Configure<TwitterCredentials>(context.Configuration.GetSection(nameof(TwitterCredentials)));
                services.AddTransient<IHashTagReportService, LoggingHashTagReportService>();
                services.AddHttpClient<TwitterHttpService>();
                services.AddHostedService<TwitterReportingService>();
            })
            .UseSerilog()
            .Build();

        host.RunAsync().Wait();
    }

    static void BuildConfig(IConfigurationBuilder builder)
    {
        builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        IConfiguration config = builder.Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(config)
            .CreateLogger();
    }
}