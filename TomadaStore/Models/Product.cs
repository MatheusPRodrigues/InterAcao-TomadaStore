using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomadaStore.Models.Models
{
    public class Product
    {
        public ObjectId Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public Category Category { get; private set; }

        public Product(
            string name, 
            string description, 
            decimal price,
            Category category
        )
        {
            Name = name;
            Description = description;
            Price = price;
            Category = category;
        }

        public Product(string id, string name, string description, decimal price, Category category)
        {
            Id = ObjectId.Parse(id);
            Name = name;
            Description = description;
            Price = price;
            Category = category;
        }

        public Product()
        {
            
        }
    }
}
