using CvCodeFirst.Data;
using CvCodeFirst.DTOs;
using CvCodeFirst.DTOs.PersonDTO;
using CvCodeFirst.DTOs.PersonDTOs;
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
            //Create a person 
            app.MapPost("/api/persons", async (CreatePersonWithDetailsDto dto, CvApiDBContext dbContext) =>
            {
                var (isValid, errors) = InputValidator.Validate(dto);
                if (!isValid) return Results.BadRequest(errors);

                var person = new Person
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    Description = dto.Description,
                    Educations = dto.Educations,
                    WorkExperiences = dto.WorkExperiences
                };

                dbContext.Person.Add(person);
                await dbContext.SaveChangesAsync();

                // Optional: return PersonDetailDto
                var result = new PersonDetailDto
                {
                    Name = person.Name,
                    Email = person.Email,
                    Phone = person.Phone,
                    Description = person.Description,
                    Educations = person.Educations,
                    WorkExperiences = person.WorkExperiences
                };

                return Results.Created($"/api/persons/{person.ID}", result);
            });

            //Get the people in the database
            app.MapGet("/api/persons", async (CvApiDBContext dbContext) =>
            {
                var persons = await dbContext.Person
                    .Include(p => p.Educations)
                    .Include(p => p.WorkExperiences)
                    .ToListAsync();

                var result = persons.Select(p => new PersonDetailDto
                {
                    Name = p.Name,
                    Email = p.Email,
                    Phone = p.Phone,
                    Description = p.Description,
                    Educations = p.Educations,
                    WorkExperiences = p.WorkExperiences
                });

                return Results.Ok(result);
            });


            //Get/Retrieve a specific person by ID 
            app.MapGet("/api/persons/{id}", async (int id, CvApiDBContext dbContext) =>
            {
                var person = await dbContext.Person
                    .Include(p => p.Educations)
                    .Include(p => p.WorkExperiences)
                    .FirstOrDefaultAsync(p => p.ID == id);
                if (person == null)
                {
                    return Results.NotFound("Entered person Id could not be found");
                }

                var result = new PersonDetailDto
                {
                    Name = person.Name,
                    Email = person.Email,
                    Phone = person.Phone,
                    Description = person.Description,
                    Educations = person.Educations,
                    WorkExperiences = person.WorkExperiences
                };

                return Results.Ok(result);
            });

            // Update a person 
            app.MapPut("/api/persons/{id}", async (int id, PersonDto dto, CvApiDBContext dbContext) =>
            {
                var person = await dbContext.Person.FindAsync(id);
                if (person == null)
                {
                    return Results.NotFound("Entered person Id could not be found");
                }

                person.Name = dto.Name;
                person.Email = dto.Email;
                person.Phone = dto.Phone;
                person.Description = dto.Description;

                await dbContext.SaveChangesAsync();

                return Results.Ok("Person updated successfully");

            });


            //Remove/Delete a person
            app.MapDelete("/api/persons/{id}", async (int id, CvApiDBContext db) =>
            {
                var person = await db.Person
                    .Include(p => p.Educations)
                    .Include(p => p.WorkExperiences)
                    .FirstOrDefaultAsync(p => p.ID == id);

                if (person == null)
                {
                    return Results.NotFound("Entered person Id could not be found");
                }

                db.Person.Remove(person);
                await db.SaveChangesAsync();

                return Results.Ok("Person updated successfully");

            });


        }
    }
}
