using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.DTOs.Product;

namespace TomadaStore.Models.DTOs.Sale
{
    public class SaleResponseConsumerDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; init; }
        public CustomerResponseConsumerDTO Customer { get; init; }
        public List<ProductResponseConsumerDTO> Products { get; init; }
        public DateTime SaleDate { get; init; }
        public decimal TotalPrice { get; init; }
        public bool IsApproved { get; init; }
    }
}
