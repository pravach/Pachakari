namespace Pachakkari.Application.DTOs.Order
{
    public class OrderResponseDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public List<OrderItemResponseDto> Products { get; set; } = new();

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; }

        // Delivery details
        public string FullName { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        public string Pincode { get; set; } = string.Empty;

        public string PaymentMethod { get; set; } = "Cash on Delivery";
    }

    public class OrderItemResponseDto
    {
        public int Id { get; set; }

        public ProductResponseDto Product { get; set; } = null!;

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
    }

    public class ProductResponseDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string Category { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;

        public string Discription { get; set; } = string.Empty;
    }
}