using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CvCodeFirst.Properties.Models
{
    public class Education
    {
        [Key]
        public int EducationID { get; set; }

        [Required]
        [MaxLength(50)]
        public required string School { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Degree { get; set; }

        public required DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }


        [ForeignKey("Person")]
        public int PersonID { get; set; }
        public Person? Persons { get; set; }
    }
}
