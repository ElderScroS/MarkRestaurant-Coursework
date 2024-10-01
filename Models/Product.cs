using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MarkRestaurant.Models;
using System.Diagnostics.CodeAnalysis;

namespace MarkRestaurant
{
    public class Product
    {
        public Product()
        {
            Id = Guid.NewGuid();
            Title = "";
            Category = "";
            Price = 0;
            ImageUrl = "/image/none.png";
        }
        public Product(string title, string category, double price, string imageUrl)
        {
            Id = Guid.NewGuid();
            Title = title;
            Category = category;
            Price = price;
            ImageUrl = imageUrl;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Category { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public string ImageUrl { get; set; }
    }
}
