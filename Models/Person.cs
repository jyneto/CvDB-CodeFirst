using System.ComponentModel.DataAnnotations;

namespace CvCodeFirst.Models
{
    public class Person
    {
        [Key]
        public int ID { get; set; }

        public required string FullName { get; set; }
       
        public required string Email { get; set; }

        public required string Phone { get; set; }

        public required string Description { get; set; }

        //Relations
        public List<Education> Educations { get; set; }
        public List<WorkExperience> WorkExperiences { get; set; }

    }
}
