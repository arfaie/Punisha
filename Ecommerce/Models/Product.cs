using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Ecommerce.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public int IdCategory { get; set; }

        [ForeignKey("IdCategory")]
        public virtual category Category { get; set; }

        public string Name { get; set; }

        public int Code { get; set; }

        public int IdUnit { get; set; }

        [ForeignKey("IdUnit")]
        public virtual Unit Unit { get; set; }

        public int Inventory { get; set; } //موجودی

        public bool Issenoble { get; set; } //قابل فروش

        public int OrderPoint { get; set; } //نقطه سفارش

        public decimal Price { get; set; }

        public decimal OldPrice { get; set; }

        public string ImageName { get; set; }
        
    }
}
