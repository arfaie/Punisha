using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Ecommerce.Models
{
    public class Addresses
    {
        [Key]
        public int Id { get; set; }

        public string IdUser { get; set; }
        [ForeignKey("IdUser")]
        public virtual ApplicationUser User { get; set; }

        //public int IdState { get; set; }
        //[ForeignKey("IdState")]
        //public virtual State State { get; set; }

        public int IdCity { get; set; }
        [ForeignKey("IdCity")]
        public virtual City City { get; set; }

        public string Address { get; set; }

        public string Plaque { get; set; }

        public long PostalCode { get; set; }

        public string Transferee { get; set; }

        public long Mobile { get; set; }

        public long Tell { get; set; }

        public double Lat { get; set; }

        public double Lan { get; set; }
    }
}
