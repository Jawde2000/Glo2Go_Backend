using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Models
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReviewID { get; set; }
        public string ReviewTraveler { get; set; } = string.Empty;
        public string ReviewSite {  get; set; } = string.Empty;
        public Site Site { get; set; }
        public string? TravelerEmail { get; set; }
        public float ReviewRating { get; set; } = 0;
        public List<string>? ReviewPics { get; set; }

    }
}
