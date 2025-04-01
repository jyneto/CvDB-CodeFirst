using CvCodeFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace CvCodeFirst.Data
{
    public class CvApiDBContext : DbContext
    {
        public CvApiDBContext(DbContextOptions<CvApiDBContext> options) : base(options)
        {
            
        }

        public DbSet<Person> Person{ get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<WorkExperience> WorkExperiences { get; set; }
        
    }
}
