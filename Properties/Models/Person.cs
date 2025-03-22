using System.ComponentModel.DataAnnotations;

namespace CvCodeFirst.Properties.Models
{
    public class Person
    {
        [Key]
        public int PersonID { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [Phone]
        public required int PhoneNumber { get; set; }


        //Relationer
        public List<Education> Educations { get; set; }
        public List<Experience> Experiences { get; set; }

    }
}
