using Newtonsoft.Json;

public class json_card
{
    [JsonProperty("cardID")]
    public int CardID { get; set; }

    [JsonProperty("cardIndex")]
    public int CardIndex { get; set; }

    [JsonProperty("turned")]
    public bool Turned { get; set; }

    [JsonProperty("locked")]
    public bool Locked { get; set; }

    public json_card(int ci, int cx, bool t, bool l) {
        CardID = ci;
        CardIndex = cx;
        Turned = t;
        Locked = l;
    }
}
