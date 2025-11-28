using System.Collections.Generic;
using Newtonsoft.Json;

namespace CS2Cheat.DTO.ClientDllDTO;

public class ClientDllDTO
{
    [JsonProperty("client.dll")]
    public ClientDllContent client_dll { get; set; }
}

public class ClientDllContent
{
    public Dictionary<string, ClassData> classes { get; set; }
}

public class ClassData
{
    public Dictionary<string, long> fields { get; set; }
}