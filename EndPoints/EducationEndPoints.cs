using CvCodeFirst.Data;
using CvCodeFirst.DTOs.EducatioDto;
using CvCodeFirst.DTOs.PersonDTO;
using CvCodeFirst.Helpers;
using CvCodeFirst.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection.Metadata.Ecma335;
using System.Xml;

namespace CvCodeFirst.EndPoints
{
    public class EducationEndPoints
    {   //Kola upp varför personId syns
        public static void RegisterEndpoints(WebApplication app) 
        {
            app.MapPost("/api/persons/{personId}/educations", async (int personId, UpdateEducationDTO dto, CvApiDBContext dbContext) =>
            {
                var (isValid, errors) = InputValidator.Validate(dto);
                if (!isValid) return Results.BadRequest(errors);

                var (personExists, personErrors) = await InputValidator.ValidatePersonExistsAsync(personId, dbContext);
                if (!personExists) return Results.BadRequest(personErrors);

                var education = new Education
                {
                    School = dto.School,
                    Degree = dto.Degree,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    PersonID = personId
                };

                dbContext.Educations.Add(education);
                await dbContext.SaveChangesAsync();

                var responseDto = new EducationDTO
                {
                    EducationID = education.EducationID,
                    School = education.School,
                    Degree = education.Degree,
                    StartDate = education.StartDate,
                    EndDate = education.EndDate
                };

                return Results.Created($"/api/persons/{personId}/educations/{education.EducationID}", responseDto);
            });

           // Get / Retrieve eduction info by id
            app.MapGet("/api/person/{personId}/educations", async (int personId, CvApiDBContext dbContext) =>
            {
                var (isValidId, idErrors) = InputValidator.ValidateId(personId);
                if (!isValidId) return Results.BadRequest(idErrors);

                var (personExists, personErrors) = await InputValidator.ValidatePersonExistsAsync(personId, dbContext);
                if (!personExists) return Results.BadRequest(personErrors);

                var educations = await dbContext.Educations
                .Where(e => e.PersonID == personId)
                .ToListAsync();
         
                var educationDto = educations.Select(e => new EducationDTO
                {
                    EducationID = e.EducationID,
                    School = e.School,
                    Degree = e.Degree,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,

                }).ToList();


                return Results.Ok(educationDto);
            });

          app.MapPut("/api/persons/{personId}/educations/{educationId}", async ( int personId, int educationId, UpdateEducationDTO dto, CvApiDBContext dbContext) =>
          {
                // Validate the education ID
                var (isValidId, idErrors) = InputValidator.ValidateId(educationId);
                if (!isValidId) return Results.BadRequest(idErrors);

                // Validate DTO content (optional if you already validate via [Required] etc.)
                var (isValid, errors) = InputValidator.Validate(dto);
                if (!isValid) return Results.BadRequest(errors);

                // Make sure the person exists
                var (personExists, personErrors) = await InputValidator.ValidatePersonExistsAsync(personId, dbContext);
                if (!personExists) return Results.BadRequest(personErrors);

                // Find the education by ID
                var education = await dbContext.Educations.FindAsync(educationId);
                if (education == null || education.PersonID != personId)
                {
                    return Results.NotFound("Education not found for the specified person.");
                }

                // Update allowed fields
                education.School = dto.School;
                education.Degree = dto.Degree;
                education.StartDate = dto.StartDate;
                education.EndDate = dto.EndDate;

                await dbContext.SaveChangesAsync();

                return Results.Ok("Education updated successfully");
            });



            app.MapDelete("/api/educations/{educationId}", async (int educationId, CvApiDBContext dbContext) =>
            {
                var (isValidId, idErrors) = InputValidator.ValidateId(educationId);
                if (!isValidId) return Results.BadRequest(idErrors);

                var education = await dbContext.Educations.FindAsync(educationId);


                if (education == null)
                {
                    return Results.NotFound("Entered education Id could not be found");
                }


                dbContext.Educations.Remove(education);
                await dbContext.SaveChangesAsync();

                return Results.Ok("Education removed successfully");
            });



        }
    }
}
