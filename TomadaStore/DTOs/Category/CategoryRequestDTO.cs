using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomadaStore.Models.DTOs.Category
{
    public class CategoryRequestDTO
    {
        [BsonElement("name")]
        public string Name { get; init; }

        [BsonElement("description")]
        public string Description { get; init; }
    }
}
