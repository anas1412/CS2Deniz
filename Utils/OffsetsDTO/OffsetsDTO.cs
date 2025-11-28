using System.Collections.Generic;
using Newtonsoft.Json;

namespace CS2Cheat.Utils.DTO;

public class OffsetsDTO
{
    [JsonProperty("client.dll")]
    public Dictionary<string, long> client_dll { get; set; }

    [JsonProperty("engine2.dll")]
    public Dictionary<string, long> engine2_dll { get; set; }

    [JsonProperty("inputsystem.dll")]
    public Dictionary<string, long> inputsystem_dll { get; set; }

    [JsonProperty("matchmaking.dll")]
    public Dictionary<string, long> matchmaking_dll { get; set; }
}