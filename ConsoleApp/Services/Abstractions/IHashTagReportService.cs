using TwitterReporter.ConsoleApp.Models;

namespace TwitterReporter.ConsoleApp.Services.Abstractions;

/// <summary>
///     Reports top hash tags
/// </summary>
public interface IHashTagReportService
{
    /// <summary>
    ///     Prints the report
    /// </summary>
    /// <param name="tweet">The tweet that was read</param>
    public void PrintReport(Tweet tweet);
}