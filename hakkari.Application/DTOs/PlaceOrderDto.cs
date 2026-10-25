using System;
using System.Collections.Generic;
using System.Text;

namespace Pachakkari.Application.DTOs
{
    public class PlaceOrderDto
    {

        public string FullName { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        public string Pincode { get; set; } = string.Empty;

        public string PaymentMethod { get; set; } = "Cash on Delivery";
    }
}
