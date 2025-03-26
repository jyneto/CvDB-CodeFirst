using CvCodeFirst.Data;
using CvCodeFirst.DTOs;
using CvCodeFirst.DTOs.PersonDTO;
using CvCodeFirst.Helpers;
using CvCodeFirst.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Xml;

namespace CvCodeFirst.EndPoints
{
    public class EducationEndPoints
    {
        public static void RegisterEndpoints(WebApplication app) 
        {

            app.MapPost("/api/educations", async (EducationDTO dto, CvApiDBContext db) =>
            {
                var (isValid, errors) = InputValidator.Validate(dto);
                if (!isValid) return Results.BadRequest(errors);

                var (personExists, personErrors) = await InputValidator.ValidatePersonExistsAsync(dto.PersonID, db);
                if (!personExists) return Results.BadRequest(personErrors);

                var education = new Education
                {
                    School = dto.School,
                    Degree = dto.Degree,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    PersonID = dto.PersonID
                };

                db.Educations.Add(education);
                await db.SaveChangesAsync();

                return Results.Created($"/api/educations/{education.EducationID}", education);
            });



            //Get/Retrieve all educations  
            app.MapGet("/educations", async (CvApiDBContext db) => {

                var educations = await db.Educations.ToListAsync();
                return Results.Ok(educations);

            });


            //Get/Retrieve eduction info by id
            app.MapGet("/api/educations/{id}", async (int id, CvApiDBContext db) =>
            {
                var (isValidId, idErrors) = InputValidator.ValidateId(id);
                if (!isValidId) return Results.BadRequest(idErrors);

                var education = await db.Educations.FindAsync(id);
                if (education == null)
                {
                    return Results.NotFound("Entered Id could not be found");
                }

                return Results.Ok(education);
            });


            app.MapPut("/api/educations/{id}", async (int id, EducationDTO dto, CvApiDBContext dbContext) =>
            {
                var (isValidId, idErrors) = InputValidator.ValidateId(id);
                if (!isValidId) return Results.BadRequest(idErrors);

                var (isValid, errors) = InputValidator.Validate(dto);
                if (!isValid) return Results.BadRequest(errors);

                var (personExists, personErrors) = await InputValidator.ValidatePersonExistsAsync(dto.PersonID, dbContext);
                if (!personExists) return Results.BadRequest(personErrors);

                var education = await dbContext.Educations.FindAsync(id);

                if (education == null)
                {
                    return Results.NotFound("Entered Id could not be found");
                }


                education.School = dto.School;
                education.Degree = dto.Degree;
                education.StartDate = dto.StartDate;
                education.EndDate = dto.EndDate;
                education.PersonID = dto.PersonID;

                await dbContext.SaveChangesAsync();

                return Results.Ok("Education updated successfully");
            });



            app.MapDelete("/api/educations/{id}", async (int id, CvApiDBContext dbContext) =>
            {
                var (isValidId, idErrors) = InputValidator.ValidateId(id);
                if (!isValidId) return Results.BadRequest(idErrors);

                var education = await dbContext.Educations.FindAsync(id);
                if (education == null)
                {
                    return Results.NotFound("Entered education could not be found");
                }

                dbContext.Educations.Remove(education);
                await dbContext.SaveChangesAsync();

                return Results.Ok("Education updated successfully");
            });



        }
    }
}
