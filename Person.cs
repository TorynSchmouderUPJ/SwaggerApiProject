using Newtonsoft.Json;

namespace PersonApi.Models {
    public class Person
    {
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("school")]
        public string School { get; set; } = string.Empty;
    }
}
