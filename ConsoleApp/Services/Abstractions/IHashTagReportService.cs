using TwitterReporter.ConsoleApp.Models;

namespace TwitterReporter.ConsoleApp.Services.Abstractions;

public interface IHashTagReportService
{
    public void PrintReport(Tweet tweet);
}