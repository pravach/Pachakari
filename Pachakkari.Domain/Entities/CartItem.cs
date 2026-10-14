using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Pachakkari.Domain.Entities
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        [JsonIgnore]
        public Cart Cart { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
