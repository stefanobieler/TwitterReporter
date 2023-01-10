using Microsoft.Extensions.Logging;
using TwitterReporter.ConsoleApp.Models;
using TwitterReporter.ConsoleApp.Services.Abstractions;
using TwitterReporter.ConsoleApp.Helper;
using System.Linq;
using System.Text;
using Serilog;

namespace TwitterReporter.ConsoleApp.Services;

public class LoggingHashTagReportService : IHashTagReportService
{
    public int TweetCount { get; private set; }
    public Dictionary<string, int> HashTags { get; private set; } = new Dictionary<string, int>();
    private readonly ILogger<LoggingHashTagReportService> _log;
    private const int MAX_SUMMARY = 10;
    public LoggingHashTagReportService(ILogger<LoggingHashTagReportService> log)
    {
        _log = log;
    }
    public void PrintReport(Tweet tweet)
    {
        string[] hash_tags = TweetParser.GetHashTag(tweet.Data.Text);

        foreach (string hash_tag in hash_tags)
        {
            if (HashTags.ContainsKey(hash_tag))
                HashTags[hash_tag] += 1;
            else
                HashTags[hash_tag] = 1;
        }
        
        PrintTopTen();
        _log.LogInformation("Total Tweets Received: {TotalTweets}", TweetCount);
        TweetCount++;
    }

    private void PrintTopTen()
    {
        string[] hash_tags = HashTags.OrderByDescending(kvp => kvp.Value).Take(MAX_SUMMARY).ToDictionary(pair=> pair.Key, pair=> pair.Value).Keys.ToArray();
        
        for(int i = 0; i < MAX_SUMMARY && i < hash_tags.Length; i++)
        {
            int rank = i + 1;
            _log.LogInformation("#{Rank}. {HashTag} - {HashTagCount}", rank, hash_tags[i], HashTags[hash_tags[i]]);
        }
    }
}