using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CvCodeFirst.Models
{
    public class WorkExperience
    {
        [Key]
        public int WorkExperienceID { get; set; }

        public required string JobTitle { get; set; }
        public required string Company { get; set; }
        public required string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [ForeignKey ("Person")]
        public int PersonID { get; set; }

        public Person? Person { get; set;  }





    }
}
