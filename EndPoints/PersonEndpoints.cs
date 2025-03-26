using CvCodeFirst.Data;
using CvCodeFirst.DTOs;
using CvCodeFirst.DTOs.PersonDTO;
using CvCodeFirst.Helpers;
using CvCodeFirst.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Security.Cryptography.Xml;

namespace CvCodeFirst.EndPoints
{
    public class PersonEndpoints
    {
        public static void RegisterEndpoints(WebApplication app)
        {
            //Get the people in the database
            app.MapGet("/persons", async (CvApiDBContext dbcontext) =>
            {
                var person = await dbcontext.Person
                    .Include(p => p.Educations)
                    .Include(p => p.WorkExperiences)
                    .Select(p => new PersonDetailDto
                    {
                        Name = p.Name,
                        Email = p.Email,
                        Description = p.Description,
                        Educations = p.Educations,
                        WorkExperiences = p.WorkExperiences

                    })
                    .ToListAsync();
                return Results.Ok(person);

            });

            //Get/Retrieve a specific person by ID 
            app.MapGet("/api/persons", async (CvApiDBContext dbContext) =>
            {
                var persons = await dbContext.Person.ToListAsync();
                return Results.Ok(persons);
            });

            app.MapGet("/api/persons/{id}", async (int id, CvApiDBContext dbContext) =>
            {
                var (isValid, errors) = InputValidator.ValidateId(id);
                if (!isValid) return Results.BadRequest(errors);

                var person = await dbContext.Person
                    .Include(p => p.Educations)
                    .Include(p => p.WorkExperiences)
                    .FirstOrDefaultAsync(p => p.ID == id);

                if (person == null) 
                {
                    return Results.NotFound();
                }

                return Results.Ok(person);

                var dto = new PersonDetailDto
                {
                    Name = person.Name,
                    Email = person.Email,
                    Description = person.Description,
                    Educations = person.Educations,
                    WorkExperiences = person.WorkExperiences
                };

                return Results.Ok(dto);
                   
            });

            //Add a new person with with details

            app.MapPost("/api/persons", async (PersonDto dto, CvApiDBContext db) =>
            {
                var (isValid, errors) = InputValidator.Validate(dto);
                if (!isValid) return Results.BadRequest(errors);

                var person = new Person
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    Description = dto.Description ?? ""
                };

                db.Person.Add(person);
                await db.SaveChangesAsync();

                return Results.Created($"/api/persons/{person.ID}", person);
            });

            // Update a person 
            app.MapPut("/api/persons/{id}", async (int id, PersonDto dto, CvApiDBContext db) =>
            {
                var (isIdValid, idErrors) = InputValidator.ValidateId(id);
                if (!isIdValid) return Results.BadRequest(idErrors);

                var (isValid, errors) = InputValidator.Validate(dto);
                if (!isValid) return Results.BadRequest(errors);

                var person = await db.Person.FindAsync(id);
                if (person is null) return Results.NotFound();

                person.Name = dto.Name;
                person.Email = dto.Email;
                person.Phone = dto.Phone;
                person.Description = dto.Description ?? "";

                await db.SaveChangesAsync();
                return Results.Ok(person);

                var responseDto = new PersonDto // Kolla Copilot
            });

            //Remove/Delete a person
            app.MapDelete("/api/persons/{id}", async (int id, CvApiDBContext db) =>
            {
                var (isValid, errors) = InputValidator.ValidateId(id);
                if (!isValid) return Results.BadRequest(errors);

                var person = await db.Person.FindAsync(id);
                if (person is null) return Results.NotFound();

                db.Person.Remove(person);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

        }
    }
}
