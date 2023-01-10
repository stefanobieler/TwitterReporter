using System.Text.RegularExpressions;

namespace TwitterReporter.ConsoleApp.Helper;

public static class TweetParser
{
    public const string RegexHashTagSearchPattern = @"\B(\#[a-zA-Z]+\b)(?!;)";
    public static string[] GetHashTag(string tweet)
    {
        MatchCollection matches = Regex.Matches(tweet, RegexHashTagSearchPattern);
        string[] result = new string[matches.Count];
        foreach (KeyValuePair<int, Match> match in matches.Select((value, i) => new KeyValuePair<int,Match>( i,value)))
        {
            result[match.Key] = match.Value.ToString();
        }

        return result;
    }
}