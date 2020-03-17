using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Ecommerce.Models
{
    public class OfferItem
    {
        [Key]
        public int Id { get; set; }

        public int IdOffer { get; set; }
        [ForeignKey("IdOffer")]
        public virtual Offer Offer { get; set; }

        public int IdProduct { get; set; }
        [ForeignKey("IdProduct")]
        public virtual Product Product { get; set; }

        public short Offertype { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal DiscountPercent { get; set; }
    }
}
