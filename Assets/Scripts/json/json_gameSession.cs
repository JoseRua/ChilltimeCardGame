using Newtonsoft.Json;

public class json_gameSession
{
    [JsonProperty("playerName")]
    public string PlayerName { get; set; }

    [JsonProperty("numMoves")]
    public int NumMoves { get; set; }

    [JsonProperty("timeElapsed")]
    public int TimeElapsed { get; set; }

    [JsonProperty("numPairs")]
    public int NumPairs { get; set; }

    [JsonProperty("Cards")]
    public json_card[] Cards { get; set; }



    public json_gameSession(string p, int nm, int np, int te, json_card[] array) {
        PlayerName = p;
        NumMoves = nm;
        NumPairs = np;
        TimeElapsed = te;
        Cards = array;
    }
}
