using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class Status
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }
    }
}
