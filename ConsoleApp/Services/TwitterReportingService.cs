using TwitterReporter.ConsoleApp.Services.HttpServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using TwitterReporter.ConsoleApp.Services.Abstractions;

namespace TwitterReporter.ConsoleApp.Services;

public class TwitterReportingService : BackgroundService
{
    private readonly ILogger<TwitterReportingService> _log;
    private readonly TwitterHttpService _httpService;
    private readonly IHashTagReportService _hashTagReportService;

    public TwitterReportingService(
        ILogger<TwitterReportingService> log,
        TwitterHttpService httpService,
        IHashTagReportService hashTagReportService)
    {
        _log = log;
        _httpService = httpService;
        _hashTagReportService = hashTagReportService;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _log.LogInformation("Twitter Reporter Started");
        return base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken cancelToken)
    {
        TwitterHttpService.OnReceivedTweet += _hashTagReportService.PrintReport;
        await _httpService.GetVolumeStreamAsync(cancelToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _log.LogInformation("Twitter Reporter Stopped...");
        return base.StopAsync(cancellationToken);
    }
}