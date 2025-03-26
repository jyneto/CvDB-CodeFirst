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
            //Get/Retrieve all educations  
            app.MapGet("/educations", async (CvApiDBContext db) => {

                var educations = await db.Educations.ToListAsync();
                return Results.Ok(educations);

            });


            //Get/Retrieve eduction info by id
            app.MapGet("/education{id}", async(int id, CvApiDBContext db) =>
            {
                var (isValid, errors) = InputValidator.ValidateId(id);

                if (!isValid) return Results.BadRequest(errors);


                var education = await db.Educations.FindAsync(id);

                if(education == null) 
                {
                    return Results.NotFound("Entered Id could not be found");
                }

                return Results.Ok(education);

            });


            app.MapPost("/persons/{id}/educations", async (int id, EducationDTO dto, CvApiDBContext db) =>
            {
                var (idValid, idErrors) = InputValidator.ValidateId(id);
                if (!idValid) return Results.BadRequest(idErrors);

                var (isValid, errors) = InputValidator.Validate(dto);
                if (!isValid) return Results.BadRequest(errors);

                var (personExists, personErrors) = await InputValidator.ValidatePersonExistsAsync(id, db);
                if (!personExists) return Results.BadRequest(personErrors);

                var education = new Education
                {
                    School = dto.School,
                    Degree = dto.Degree,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    PersonID = id
                };

                db.Educations.Add(education);
                await db.SaveChangesAsync();
                return Results.Created($"/educations/{education.EducationID}", dto);
            });

            app.MapDelete("/persons/{id}/educations/{educationId}", async (int id, int educationId, CvApiDBContext db) =>
            {
                var (idValid, idErrors) = InputValidator.ValidateId(id);
                var (educationValid, educationErrors) = InputValidator.ValidateId(educationId);
                if (!idValid || !educationValid) return Results.BadRequest(idErrors.Concat(educationErrors));

                var education = await db.Educations
                    .FirstOrDefaultAsync(e => e.PersonID == id && e.EducationID == educationId);

                if (education is null)
                    return Results.NotFound("Education not found");

                db.Educations.Remove(education);
                await db.SaveChangesAsync();

                return Results.NoContent();
            });

            app.MapPut("/educations/{educationId}", async (int educationId, EducationDTO dto, CvApiDBContext db) =>
            {
                var (idValid, idErrors) = InputValidator.ValidateId(educationId);
                if (!idValid) return Results.BadRequest(idErrors);

                var (isValid, errors) = InputValidator.Validate(dto);
                if (!isValid) return Results.BadRequest(errors);

                var education = await db.Educations.FindAsync(educationId);
                if (education is null) return Results.NotFound("Education not found");

                education.Degree = dto.Degree;
                education.School = dto.School;
                education.StartDate = dto.StartDate;
                education.EndDate = dto.EndDate;

                await db.SaveChangesAsync();
                return Results.Ok("Education updated successfully");
            });

            ////Add education data 
            //app.MapPost("/persons/{id}/educations" , async (int id , EducationDTO dto , CvApiDBContext db) => 
            //{
            //    var (isValid, errors) = InputValidator.Validate(dto);
            //    if (!isValid) return Results.BadRequest(errors);

            //    var (personExists, personErrors) = await InputValidator.ValidatePersonExistsAsync(dto.PersonID, db);
            //    if (!personExists) return Results.BadRequest(personErrors);

            //    var education = new Education
            //    {
            //        School = dto.School,
            //        Degree = dto.Degree,
            //        StartDate = dto.StartDate,
            //        EndDate = dto.EndDate,
            //        PersonID = dto.PersonID
            //    };

            //    db.Educations.Add(education);
            //    await db.SaveChangesAsync();
            //    return Results.Created($"/education/{education.EducationID}", education);
            //});


            ////Update education info related to a specific ID
            //app.MapPut("/education/{id}", async (int id, EducationDTO dto, CvApiDBContext db) =>
            //{
            //    var (isIdValid, idErrors) = InputValidator.ValidateId(id);
            //    if (!isIdValid) return Results.BadRequest(idErrors);

            //    var (isDtoValid, errors) = InputValidator.Validate(dto);
            //    if (!isDtoValid) return Results.BadRequest(errors);

            //    var educationFromDb = await db.Educations.FindAsync(id);
            //    if (educationFromDb is null) return Results.NotFound();

            //    educationFromDb.School = dto.School;
            //    educationFromDb.Degree = dto.Degree;
            //    educationFromDb.StartDate = dto.StartDate;
            //    educationFromDb.EndDate = dto.EndDate;
            //    educationFromDb.PersonID = dto.PersonID;

            //    await db.SaveChangesAsync();
            //    return Results.Ok(dto);

            //});


            ////Remove/Delete education info related to a specific ID/person?
            //app.MapDelete("/education/{id}", async (int id , CvApiDBContext db) => 
            //{
            //    var (isIdValid, errors) = InputValidator.ValidateId(id);
            //    if (!isIdValid) return Results.BadRequest(errors);

            //    var educationFromDb = await db.Educations.FindAsync(id);
            //    if (educationFromDb is null) return Results.NotFound();

            //    db.Educations.Remove(educationFromDb);
            //    await db.SaveChangesAsync();
            //    return Results.NoContent();

            //});



        }
    }
}
