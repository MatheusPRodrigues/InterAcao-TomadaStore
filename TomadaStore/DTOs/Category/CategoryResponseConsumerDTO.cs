using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TomadaStore.Models.DTOs.Category
{
    public class CategoryResponseConsumerDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        
        public ObjectId Id { get; init; }

        public string Name { get; init; }

        public string Description { get; init; }
    }
}
