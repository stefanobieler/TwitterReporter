using Newtonsoft.Json;

namespace TwitterReporter.ConsoleApp.Models;

public class TweetData : ICloneable
{
    [JsonProperty("edit_history_tweet_ids")]
    public string[] EditHistoryTweetIds { get; set; } = { };

    [JsonProperty("id")] 
    public string Id { get; set; } = string.Empty;
    [JsonProperty("text")]
    public string Text { get; set; } = string.Empty;

    public object Clone()
    {
        TweetData data = (TweetData) MemberwiseClone();
        data.EditHistoryTweetIds = (string[]) EditHistoryTweetIds.Clone();
        data.Id = (string) Id.Clone();
        data.Text = (string) Text.Clone();
        return data;
    }
}