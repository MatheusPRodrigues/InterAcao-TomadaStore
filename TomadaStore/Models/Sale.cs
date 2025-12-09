using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomadaStore.Models.Models
{
    public class Sale
    {
        public ObjectId Id { get; private set; }
        public Customer Customer { get; private set; }
        public List<Product> Products { get; private set; }
        public DateTime SaleDate { get; private set; }
        public decimal TotalPrice { get; private set; }
        public bool IsApproved { get; private set; }

        public Sale(
            Customer customer,
            List<Product> products
        )
        {
            Id = ObjectId.GenerateNewId();
            Customer = customer;
            Products = products;
            SaleDate = DateTime.UtcNow;
            TotalPrice = products.Sum(p => p.Price);
            IsApproved = false;
        }

        public Sale(
            ObjectId id,
            Customer customer,
            List<Product> products,
            DateTime saleDate,
            decimal totalPrice,
            bool isApproved
        )
        {
            Id = id;
            Customer = customer;
            Products = products;
            SaleDate = saleDate;
            TotalPrice = totalPrice;
            IsApproved = isApproved;
        }

        public void SetIsApproved(bool isApproved)
        {
            IsApproved = isApproved;
        }
    }
}
