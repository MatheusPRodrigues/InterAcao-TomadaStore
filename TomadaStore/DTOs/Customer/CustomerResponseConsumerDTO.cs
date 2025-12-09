using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TomadaStore.Models.DTOs.Customer
{
    public class CustomerResponseConsumerDTO
    {
        public int Id { get; init; }

        public string FirstName { get; init; }

        public string LastName { get; init; }

        public string Email { get; init; }

        public string? PhoneNumber { get; init; }
        public bool Status { get; set; }
    }
}
