using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Ecommerce.Models
{
    public class Agency
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        //public int IdState { get; set; }

        //[ForeignKey("IdState")]
        //public virtual State State { get; set; }

        public int IdCity { get; set; }

        [ForeignKey("IdCity")]
        public virtual City City { get; set; }

        public string Address { get; set; }

        public string Plaque { get; set; }

        public long PostalCode { get; set; }

        public long Tell { get; set; }


    }
}
