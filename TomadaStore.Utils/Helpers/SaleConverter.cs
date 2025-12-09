using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TomadaStore.Models.DTOs.Sale;
using System.Threading.Tasks;
using TomadaStore.Models.Models;

namespace TomadaStore.Utils.Helpers
{
    public class SaleConverter
    {
        public Sale ConvertForSale(SaleResponseConsumerDTO dto)
        {

            var customer = new Customer(
                dto.Customer.Id,
                dto.Customer.FirstName,
                dto.Customer.LastName,
                dto.Customer.Email,
                dto.Customer.PhoneNumber,
                dto.Customer.Status
            );

            var products = new List<Product>();

            foreach (var p in dto.Products)
            {
                var product = new Product(
                    p.Id.ToString(),
                    p.Name,
                    p.Description,
                    p.Price,
                    new Category(
                        p.Category.Id.ToString(),
                        p.Category.Name,
                        p.Category.Description
                    )
                );
                products.Add(product);
            }

            var sale = new Sale(
                dto.Id,
                customer,
                products,
                dto.SaleDate,
                dto.TotalPrice,
                dto.IsApproved
            );

            return sale;
        }
    }
}
