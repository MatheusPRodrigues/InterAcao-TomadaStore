using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TomadaStore.Models.DTOs.Category;

namespace TomadaStore.Models.DTOs.Product
{
    public class ProductResponseConsumerDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; init; }

        public string Name { get; init; }

        
        public string Description { get; init; }


        public decimal Price { get; init; }

        public CategoryResponseConsumerDTO Category { get; init; }
    }
}
