
using Newtonsoft.Json;

public class json_score
{
    [JsonProperty("PlayerName")]
    public string PlayerName { get; set; }
    [JsonProperty("Score")]
    public int Score { get; set; }
    [JsonProperty("Time")]
    public string Time { get; set; }

    public json_score(string p, int s, string t) {
        PlayerName = p;
        Score = s;
        Time = t;
    }
}
