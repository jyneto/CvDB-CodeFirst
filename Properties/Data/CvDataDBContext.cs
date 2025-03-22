using CvCodeFirst.Properties.Models;
using Microsoft.EntityFrameworkCore;

namespace CvCodeFirst.Properties.Data
{
    public class CvDataDBContext : DbContext
    {
        public CvDataDBContext(DbContextOptions<CvDataDBContext> options) : base(options)
        {
            
        }

        public DbSet<Person> Persons{ get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Experience> Experiences { get; set; }

    }
}
