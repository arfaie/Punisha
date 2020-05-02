using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ECommerce.Models
{
    public class AboutUs
    {
        [Display(Name = "شناسه")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Display(Name = "درباره ما")]
        public string AboutUss { get; set; }

        [Display(Name = "اتباط با ما")]
        public string ContactUs { get; set; }
    }
}
