

using Dapper.Contrib.Extensions;
using Newtonsoft.Json;


namespace PokemonAPIusingDapper.Models
{
    [Table("Pokemon")]
    public class Pokemon
    {
        [JsonProperty("id")] 
        public int Id { get; set; }

        [JsonProperty("num")]
        public string Num { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public List<string> Type { get; set; } = new List<string>();

        [JsonProperty("prev_evolution")]
        public List<Evolution> PrevEvolution { get; set; }

        [JsonProperty("next_evolution")]
        public List<Evolution> NextEvolution { get; set; }

        // Construtor
        public Pokemon()
        {
            Type = new List<string>();
            PrevEvolution = new List<Evolution>();
            NextEvolution = new List<Evolution>();
        }

        public override string ToString()
        {
            return $"Pokemon [id={Id}, num={Num}, name={Name}, type={string.Join(", ", Type)}, prev_evolution={PrevEvolution}, next_evolution={NextEvolution}]";
        }

    }
}