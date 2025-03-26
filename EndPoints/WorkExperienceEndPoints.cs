using CvCodeFirst.Data;
using CvCodeFirst.DTOs;
using CvCodeFirst.Helpers;
using CvCodeFirst.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CvCodeFirst.EndPoints
{
    public class WorkExperienceEndPoints
    {
        public static void RegisterEndpoints(WebApplication app)
        {
            //Add work experience data related to a person
            app.MapPost("/api/workexperiences", async (WorkExperienceDTO dto, CvApiDBContext dbContext) =>
            {
                var (isValid, errors) = InputValidator.Validate(dto);
                if (!isValid) return Results.BadRequest(errors);

                var (personExists, personErrors) = await InputValidator.ValidatePersonExistsAsync(dto.PersonID, dbContext);
                if (!personExists) return Results.BadRequest(personErrors);

                var experience = new WorkExperience
                {
                    JobTitle = dto.JobTitle,
                    Company = dto.Company,
                    Description = dto.Description,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    PersonID = dto.PersonID
                };

                dbContext.WorkExperiences.Add(experience);
                await dbContext.SaveChangesAsync();

                return Results.Created($"/api/workexperiences/{experience.WorkExperienceID}", experience);
            });

            //Get/Retrieve all work experience
            app.MapGet("/workexperiences", async (CvApiDBContext db) =>
            {
                var experiences = await db.WorkExperiences.ToListAsync();
                return Results.Ok(experiences);
            });


            //Get/Retrieve work experience 
            app.MapGet("/api/workexperiences/{id}", async (int id, CvApiDBContext dbContext) =>
            {
                var (isValidId, idErrors) = InputValidator.ValidateId(id);
                if (!isValidId) return Results.BadRequest(idErrors);

                var experience = await dbContext.WorkExperiences.FindAsync(id);
                if (experience == null)
                {
                    return Results.NotFound("Entered experience Id could not be found");
                }

                return Results.Ok(experience);
            });


            //Update work experience info related to a specific wokexperience id and specific person
            app.MapPut("/api/workexperiences/{id}", async (int id, WorkExperienceDTO dto, CvApiDBContext dbContext) =>
            {
                var (isValidId, idErrors) = InputValidator.ValidateId(id);
                if (!isValidId) return Results.BadRequest(idErrors);

                var (isValid, errors) = InputValidator.Validate(dto);
                if (!isValid) return Results.BadRequest(errors);

                var (personExists, personErrors) = await InputValidator.ValidatePersonExistsAsync(dto.PersonID, dbContext);
                if (!personExists) return Results.BadRequest(personErrors);

                var experience = await dbContext.WorkExperiences.FindAsync(id);
                if (experience == null)
                {
                    return Results.NotFound("Entered experience Id could not be found");
                }

                experience.JobTitle = dto.JobTitle;
                experience.Company = dto.Company;
                experience.Description = dto.Description;
                experience.StartDate = dto.StartDate;
                experience.EndDate = dto.EndDate;
                experience.PersonID = dto.PersonID;

                await dbContext.SaveChangesAsync();

                return Results.Ok("Work experience updated successfully");

            });


            //Remove/Delete experience
            app.MapDelete("/api/workexperiences/{id}", async (int id, CvApiDBContext dbContext) =>
            {
                var (isValidId, idErrors) = InputValidator.ValidateId(id);
                if (!isValidId) return Results.BadRequest(idErrors);

                var experience = await dbContext.WorkExperiences.FindAsync(id);
                if (experience == null)
                {
                    return Results.NotFound("Entered experience could not be found");
                }
                dbContext.WorkExperiences.Remove(experience);
                await dbContext.SaveChangesAsync();

                return Results.Ok("Work experience updated successfully");
            });

        }
    }
}
