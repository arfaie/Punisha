using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Ecommerce.Models
{
    public class ProductGallery
    {
        [Key]
        public int Id { get; set; }

        public int IdProduct { get; set; }

        [ForeignKey("IdProduct")]
        public virtual Product Product { get; set; }

        public string Image { get; set; }
    }
}
