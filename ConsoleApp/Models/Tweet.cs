using System.Security.Principal;
using Newtonsoft.Json;

namespace TwitterReporter.ConsoleApp.Models;

public class Tweet : ICloneable
{
    [JsonProperty("data")]
    public TweetData? Data { get; set; }

    public object Clone()
    {
        Tweet tweet = (Tweet) MemberwiseClone();
        tweet.Data = (TweetData) Data.Clone();

        return tweet;
    }
}