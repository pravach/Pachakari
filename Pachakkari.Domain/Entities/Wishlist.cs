using System;
using System.Collections.Generic;
using System.Text;

namespace Pachakkari.Domain.Entities
{
    public class Wishlist
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public ICollection<WishlistItem> WishlistItems { get; set; }
        = new List<WishlistItem>();

    }
}
