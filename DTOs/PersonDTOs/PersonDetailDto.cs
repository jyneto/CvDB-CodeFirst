using CvCodeFirst.DTOs.EducatioDto;
using CvCodeFirst.DTOs.WorkExperienceDtos;
using CvCodeFirst.Models;

namespace CvCodeFirst.DTOs.PersonDTO
{
    public class PersonDetailDto
    {
        //To Get and show a persons info with education and workexperience
        public string FullName { get; set; }
        public string  Email { get; set; }
        public string  Phone { get; set; }
        public string Description { get; set; }

        public List<EducationDTO> Educations { get; set; }
        public List<WorkExperienceDTO> WorkExperiences { get; set; }
    }
}
