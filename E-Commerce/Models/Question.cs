using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ECommerce.Models
{
    public class Question
    {
        [Display(Name = "شناسه")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Display(Name = "سوال")]
        public string Questions { get; set; }

        [Display(Name = "پاسخ")]
        public string Answer { get; set; }

        [Display(Name = "وضعیت")]
        public bool Accepted { get; set; }
    }
}
