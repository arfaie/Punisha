using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Ecommerce.Models
{
    public class Field
    {
        [Key]
        public int Id { get; set; }

        public int? IdSelectGroup { get; set; }
        [ForeignKey("IdSelectGroup")]
        public virtual SelectGroup SelectGroupList { get; set; }

        public int IdFieldGroup { get; set; }
        [ForeignKey("IdFieldGroup")]
        public virtual FieldGroup SelectFieldGroupList { get; set; }

        //public int IdCategory { get; set; }
        //[ForeignKey("IdCategory")]
        //public virtual category SelectCategoryList { get; set; }

        public short Type { get; set; }

        public string Title { get; set; }
    }
}
