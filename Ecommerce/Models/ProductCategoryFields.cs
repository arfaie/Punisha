using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Ecommerce.Models
{
    public class ProductCategoryFields
    {
        [Key]
        public int Id { get; set; }


        public int IdCategory { get; set; }
        [ForeignKey("IdCategory")]
        public virtual category Categorylist { get; set; }

        public int IdFaild { get; set; }
        [ForeignKey("IdFaild")]
        public virtual Field FieldList { get; set; }


    }
}
