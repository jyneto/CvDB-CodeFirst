using CvCodeFirst.Data;
using CvCodeFirst.DTOs.WorkExperienceDtos;
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
            app.MapPost("api/persons/{personId}/workexperiences", async (int personId, UpdateWorkExperienceDTO dto, CvApiDBContext dbContext) =>
            {
                var (isValid, errors) = InputValidator.Validate(dto);
                if (!isValid) return Results.BadRequest(errors);

                var (personExists, personErrors) = await InputValidator.ValidatePersonExistsAsync(personId, dbContext);
                if (!personExists) return Results.BadRequest(personErrors);

                var experience = new WorkExperience
                {
                    JobTitle = dto.JobTitle,
                    Company = dto.Company,
                    Description = dto.Description,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    PersonID = personId
                };

                dbContext.WorkExperiences.Add(experience);
                await dbContext.SaveChangesAsync();

                var responseDto = new WorkExperienceDTO
                {
                    WorkExperienceID = experience.WorkExperienceID,
                    JobTitle = experience.JobTitle,
                    Company = experience.Company,
                    Description = experience.Description,
                    StartDate = experience.StartDate,
                    EndDate = experience.EndDate
                };

                return Results.Created($"/api/persons/{personId}/workexperiences/{experience.WorkExperienceID}", responseDto);
            });



            //Get/Retrieve work experience 
            app.MapGet("/api/person/{personId}/workexperiences", async (int personId, CvApiDBContext dbContext) =>
            {
                var (isValidId, idErrors) = InputValidator.ValidateId(personId);
                if (!isValidId) return Results.BadRequest(idErrors);

                var (personExists, personErrors) = await InputValidator.ValidatePersonExistsAsync(personId, dbContext);
                if (!personExists) return Results.BadRequest(personErrors);

                var experiences = await dbContext.WorkExperiences
                .Where(e => e.PersonID == personId)
                .ToListAsync();

                if (experiences == null)
                {
                    return Results.NotFound("Entered work experience Id could not be found");
                }

                var experienceDto = experiences.Select(w => new WorkExperienceDTO
                {
                    WorkExperienceID = w.WorkExperienceID,
                    JobTitle = w.JobTitle,
                    Company = w.Company,
                    Description = w.Description,
                    StartDate = w.StartDate,
                    EndDate = w.EndDate

                }).ToList();

                return Results.Ok(experienceDto);
            });


            //Update work experience info related to a specific wokexperience experienceId and specific person
            app.MapPut("/api/persons/{personId}/workexperiences/{experienceId}", async (int personId,int experienceId, UpdateWorkExperienceDTO dto, CvApiDBContext dbContext) =>
            {
                var (isValidId, idErrors) = InputValidator.ValidateId(personId);
                if (!isValidId) return Results.BadRequest(idErrors);

                var (isValid, errors) = InputValidator.Validate(dto);
                if (!isValid) return Results.BadRequest(errors);

                var (personExists, personErrors) = await InputValidator.ValidatePersonExistsAsync(personId, dbContext);
                if (!personExists) return Results.BadRequest(personErrors);

                var experience = await dbContext.WorkExperiences.FindAsync(experienceId);
                if (experience == null)
                {
                    return Results.NotFound("Entered work experience Id could not be found");
                }

                experience.JobTitle = dto.JobTitle;
                experience.Company = dto.Company;
                experience.Description = dto.Description;
                experience.StartDate = dto.StartDate;
                experience.EndDate = dto.EndDate;

                await dbContext.SaveChangesAsync();

                return Results.Ok("Work experience updated successfully");

            });


            //Remove/Delete experience
            app.MapDelete("/api/workexperiences/{personId}/{experienceId}", async (int personId,int experienceId, CvApiDBContext dbContext) =>
            {
                var (isPersonValid, personIdErrors) = InputValidator.ValidateId(personId);
                if (!isPersonValid) return Results.BadRequest(personIdErrors);

                var (isValidId, idErrors) = InputValidator.ValidateId(experienceId);
                if (!isValidId) return Results.BadRequest(idErrors);

                // Make sure the person exists
                var (personExists, personErrors) = await InputValidator.ValidatePersonExistsAsync(personId, dbContext);
                if (!personExists) return Results.BadRequest(personErrors);

                // Check if education exists for the person
                var experience = await dbContext.WorkExperiences
                    .FirstOrDefaultAsync(w => w.WorkExperienceID == experienceId && w.PersonID == personId);

                if (experience == null)
                {
                    return Results.NotFound("Entered experience could not be found");
                }

                dbContext.WorkExperiences.Remove(experience);
                await dbContext.SaveChangesAsync();

                return Results.Ok("Work experience removed successfully");
            });

        }
    }
}
