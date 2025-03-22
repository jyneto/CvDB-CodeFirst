using System.ComponentModel.DataAnnotations;

namespace CvCodeFirst.Properties.Models
{
    public class Experience
    {
        [Key]
        public int ExperienceID { get; set; }

        public required string JobTitle { get; set; }
        public required string Company { get; set; }

        public required string Description { get; set; }
        public required int Year { get; set; }


    }
}
