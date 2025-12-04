using MongoDB.Bson;

namespace TomadaStore.Models.Models
{
    public class Category
    {
        public ObjectId Id { get; private set; } 
        public string Name { get; private set; }
        public string Description { get; private set; }


        public Category(
            string name,
            string description
        )
        {
            Id = ObjectId.GenerateNewId(); 
            Name = name;
            Description = description;
        }

        public Category(string id, string name, string description)
        {
            Id = ObjectId.Parse(id);
            Name = name;
            Description = description;
        }

        public Category()
        {
            
        }
    }
}