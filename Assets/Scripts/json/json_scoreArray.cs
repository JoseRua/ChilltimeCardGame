using Newtonsoft.Json;

public class json_scoreArray
{
    [JsonProperty("scores")]
    public json_score[] Scores { get; set; }
}
