using System.Text.Json.Serialization;

namespace StoreApi.Models
{
    public class Product
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonPropertyName("category")]
        public string? Category { get; set; }
        [JsonPropertyName("image")]
        public string? Image { get; set; }
        [JsonPropertyName("rate")]
        public Rating? Rating { get; set; }
    }

    public class Rating
    {
        public double Rate { get; set; }
        public int Count { get; set; }
    }
}