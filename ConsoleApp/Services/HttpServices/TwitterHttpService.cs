using System.Net;
using TwitterReporter.ConsoleApp.Options;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Text.Encodings;
using System.Text.Unicode;
using Newtonsoft.Json;
using TwitterReporter.ConsoleApp.Models;


namespace TwitterReporter.ConsoleApp.Services.HttpServices
{
    public class TwitterHttpService
    {
        
        public static event Action<Tweet>? OnReceivedTweet;
        
        private readonly ILogger<TwitterHttpService> _log;
        private readonly HttpClient _httpClient;

        public TwitterHttpService(
            ILogger<TwitterHttpService> log,
            HttpClient httpClient,
            IOptions<TwitterCredentials> twitterCredentials)
        {
            _log = log;
            _httpClient = httpClient;

            _httpClient.BaseAddress = new Uri("https://api.twitter.com");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", twitterCredentials.Value.BearerToken);
        }

        public async Task GetVolumeStreamAsync(CancellationToken cancelToken)
        {
            Stream response_stream = await _httpClient.GetStreamAsync("2/tweets/sample/stream", cancelToken);

            using (StreamReader reader = new StreamReader(response_stream))
            {
                while (!reader.EndOfStream)
                {
                    string? line = await reader.ReadLineAsync();
                    if (string.IsNullOrWhiteSpace(line) == false)
                    {
                        Tweet? tweet = JsonConvert.DeserializeObject<Tweet>(line);
                        if (tweet != null)
                        {
                            _log.LogDebug("ID: {Id}: {Text}", tweet.Data?.Id, tweet.Data?.Text);
                            OnReceivedTweet?.Invoke((Tweet)tweet.Clone());
                        }
                    }
                }
            }
        }
    }
}