﻿using System.ComponentModel.DataAnnotations;

namespace CvCodeFirst.DTOs.EducatioDto
{
    public class EducationDTO
    {
        [Required]
        public int EducationID { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public required string School { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Degree { get; set; }

        [Required]
        public required DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
