using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TomadaStore.Models.DTOs.Category;
using TomadaStore.Models.Models;

namespace TomadaStore.Models.DTOs.Product
{
    public class ProductResponseDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("Id")]
        public string Id { get; init; }

        [BsonElement("name")]
        [JsonPropertyName("Name")]
        public string Name { get; init; }

        [BsonElement("description")]
        [JsonPropertyName("Description")]
        public string Description { get; init; }

        [BsonElement("price")]
        [JsonPropertyName("Price")]
        public decimal Price { get; init; }

        [BsonElement("category")]
        [JsonPropertyName("Category")]
        public CategoryResponseDTO Category { get; init; }
    }
}
