
using CvCodeFirst.Data;
using CvCodeFirst.EndPoints;
using Microsoft.EntityFrameworkCore;

namespace CvCodeFirst
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<CvApiDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            PersonEndpoints.RegisterEndpoints(app);
            //EducationEndpoints.RegisterEndpoints(app); 
            //WorkExperienceEndpoints.RegisterEndpoints(app);
            GitHubReposEndpoints.RegisterEndpoints(app);


            app.Run();


            //To do: ändra Endpoints till motsvarande ändrade filnamn, kommenterade ut den uppdaterade namn
            //
        }
    }
}
