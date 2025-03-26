using CvCodeFirst.Data;
using CvCodeFirst.DTOs;
using CvCodeFirst.Helpers;
using CvCodeFirst.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Runtime.InteropServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CvCodeFirst.EndPoints
{
    public class WorkExperienceEndPoints
    {
        public static void RegisterEndpoints(WebApplication app)
        {

            //Get/Retrieve all work experience
            app.MapGet("/workexperiences", async (CvApiDBContext db) =>
            {
                var experiences = await db.WorkExperiences.ToListAsync();
                return Results.Ok(experiences);
            });


            //Get/Retrieve work experience 
            app.MapGet("/workexperience/{id}", async (CvApiDBContext db, int id) =>
            {
                var (isValid, errors) = InputValidator.ValidateId(id);
                if (!isValid) return Results.BadRequest(errors);

                var experienceFromDb = await db.WorkExperiences.FindAsync(id);

                if (experienceFromDb is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(experienceFromDb);

            });

                //Add work experience data related to a person
                app.MapPost("/persons/{id}/workexperiences", async (int id, WorkExperienceDTO dto, CvApiDBContext db) =>
            {
                var (idValid, idErrors) = InputValidator.ValidateId(id);
                if (!idValid) return Results.BadRequest(idErrors);

                var (isValid, errors) = InputValidator.Validate(dto);
                if (!isValid) return Results.BadRequest(errors);

                var (personExists, personErrors) = await InputValidator.ValidatePersonExistsAsync(id, db);
                if (!personExists) return Results.BadRequest(personErrors);

                var experience = new WorkExperience
                {
                    JobTitle = dto.JobTitle,
                    Company = dto.Company,
                    Description = dto.Description ?? "",
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    PersonID = id
                };

                db.WorkExperiences.Add(experience);
                await db.SaveChangesAsync();

                return Results.Created($"/workexperiences/{experience.WorkExperienceID}", dto);
            });

            //Update work experience info related to a specific wokexperience id and specific person
            app.MapPut("/workexperiences/{experienceId}", async (int experienceId, WorkExperienceDTO dto, CvApiDBContext db) =>
            {
                var (expValid, expErrors) = InputValidator.ValidateId(experienceId);
                if (!expValid) return Results.BadRequest(expErrors);

                var (isValid, errors) = InputValidator.Validate(dto);
                if (!isValid) return Results.BadRequest(errors);

                var experience = await db.WorkExperiences.FindAsync(experienceId);
                if (experience is null) return Results.NotFound("Work experience not found");

                experience.JobTitle = dto.JobTitle;
                experience.Company = dto.Company;
                experience.Description = dto.Description ?? "";
                experience.StartDate = dto.StartDate;
                experience.EndDate = dto.EndDate;

                await db.SaveChangesAsync();
                return Results.Ok("Work experience updated successfully");
            });

            //Remove/Delete experience
            app.MapDelete("/persons/{id}/workexperiences/{experienceId}", async (int id, int experienceId, CvApiDBContext db) =>
            {
                var (idValid, idErrors) = InputValidator.ValidateId(id);
                var (expValid, expErrors) = InputValidator.ValidateId(experienceId);
                if (!idValid || !expValid) return Results.BadRequest(idErrors.Concat(expErrors));

                var experience = await db.WorkExperiences
                    .FirstOrDefaultAsync(e => e.PersonID == id && e.WorkExperienceID == experienceId);

                if (experience is null)
                    return Results.NotFound("Work experience not found");

                db.WorkExperiences.Remove(experience);
                await db.SaveChangesAsync();

                return Results.NoContent();
            });

      
         


            ////Get/Retrieve work experience 
            //app.MapGet("/workexperiences/{id}", async (CvApiDBContext db, int id) =>
            //{
            //    var (isValid, errors) = InputValidator.ValidateId(id);
            //    if (!isValid) return Results.BadRequest(errors);

            //    var experiencesFromDb = await db.WorkExperiences.FindAsync();

            //    if (experiencesFromDb is null)
            //    {
            //        return Results.NotFound();
            //    }

            //    return Results.Ok(experiencesFromDb);


            //});

            ////Add work experience data related to a person
            //app.MapPost("/persons/{personId}workexperiences", async (int personId, WorkExperienceDTO dto, CvApiDBContext db) =>
            //{
            //    var (isValid, errors) = InputValidator.Validate(dto);
            //    if (!isValid) return Results.BadRequest(errors);

            //    var (personExists, personErrors) = await InputValidator.ValidatePersonExistsAsync(personId, db);
            //    if (!personExists) return Results.BadRequest(personErrors);

            //    var experience = new WorkExperience
            //    {
            //        JobTitle = dto.JobTitle,
            //        Company = dto.Company,
            //        Description = dto.Description ?? "",
            //        StartDate = dto.StartDate,
            //        EndDate = dto.EndDate,
            //        PersonID = personId

            //    };

            //    db.WorkExperiences.Add(experience);
            //    await db.SaveChangesAsync();
            //    return Results.Created($"/workexperiences/{experience.WorkExperienceID}", experience);
            //});


            ////Update work experience info related to a specific wokexperience id and specific person
            //app.MapPut("/persons/{personId}workexperiences/{id}" , async (int personId , int id,WorkExperienceDTO dto, CvApiDBContext db) => 
            //{
            //    var (isValid, errors) = InputValidator.ValidateId(id);
            //    if (!isValid) return Results.BadRequest(errors);

            //    var (isDtoValid, dtoErrors) = InputValidator.Validate(dto);
            //    if (!isDtoValid) return Results.BadRequest(dtoErrors);

            //    var (personExists, personErrors) = await InputValidator.ValidatePersonExistsAsync(personId, db);
            //    if (!personExists) return Results.BadRequest(personErrors);

            //    //Find the work experience record
            //    var experience = await db.WorkExperiences.FirstOrDefaultAsync(e => e.WorkExperienceID == id && e.PersonID == personId);
            //    if (experience is null) return Results.NotFound();

            //    //Update the work experience fields
            //    experience.JobTitle = dto.JobTitle;
            //    experience.Company = dto.Company;
            //    experience.Description = dto.Description ?? "";
            //    experience.StartDate = dto.StartDate;
            //    experience.EndDate = dto.EndDate;


            //    await db.SaveChangesAsync();
            //    return Results.Ok(experience);

            //});

            // info related to a specific work experience id
            //app.MapDelete("/workexperiences/{id}" , async(int id, CvApiDBContext db) => 
            //{
            //    var (isValid, errors) = InputValidator.ValidateId(id);
            //    if (!isValid) return Results.BadRequest(errors);

            //    var experience = await db.WorkExperiences.FindAsync(id);
            //    if (experience is null) return Results.NotFound();

            //    db.WorkExperiences.Remove(experience);
            //    await db.SaveChangesAsync();
            //    return Results.NoContent();


            //});

        }
    }
}
