using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MarkRestaurant.Models
{
    public class Order
    {
        public Order()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public string UserId { get; set; }
        public Guid ProductId {  get; set; }
        public User User { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
