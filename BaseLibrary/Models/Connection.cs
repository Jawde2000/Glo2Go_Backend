using System.Text.Json.Serialization;

namespace BaseLibrary.Models
{
    public class Connection
    {

        [JsonIgnore]
        internal List<Traveler>? Users { get; set; }
    }
}
