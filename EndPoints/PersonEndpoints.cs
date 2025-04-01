using CvCodeFirst.Data;
using CvCodeFirst.DTOs;
using CvCodeFirst.DTOs.EducatioDto;
using CvCodeFirst.DTOs.PersonDTO;
using CvCodeFirst.DTOs.PersonDTOs;
using CvCodeFirst.DTOs.WorkExperienceDtos;
using CvCodeFirst.Helpers;
using CvCodeFirst.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security.Cryptography.Xml;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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

                // Ensure Educations and WorkExperiences are not empty if needed
                if (!dto.Educations.Any())
                {
                    errors.Add("At least one education must be provided.");
                    return Results.BadRequest(errors);
                }

                if (!dto.WorkExperiences.Any())
                {
                    errors.Add("At least one work experience must be provided.");
                    return Results.BadRequest(errors);
                }

                var person = new Person
                {
                    FullName = dto.FullName,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    Description = dto.Description,
                    Educations = dto.Educations.Select(e => new Education
                    {
                        School = e.School,
                        Degree = e.Degree,
                        StartDate = e.StartDate,
                        EndDate = e.EndDate
                    }).ToList(),
                    WorkExperiences = dto.WorkExperiences.Select(w => new WorkExperience
                    {
                        JobTitle = w.JobTitle,
                        Company = w.Company,
                        Description = w.Description,
                        StartDate = w.StartDate,
                        EndDate = w.EndDate
                    }).ToList()
                };

                dbContext.Person.Add(person);
                await dbContext.SaveChangesAsync();

                var result = new PersonDetailDto
                {
                    FullName = person.FullName,
                    Email = person.Email,
                    Phone = person.Phone,
                    Description = person.Description,
                    Educations = person.Educations.Select(e => new EducationDTO
                    {
                        EducationID = e.EducationID,
                        School = e.School,
                        Degree = e.Degree,
                        StartDate = e.StartDate,
                        EndDate = e.EndDate
                    }).ToList(),
                    WorkExperiences = person.WorkExperiences.Select(w => new WorkExperienceDTO
                    {
                        WorkExperienceID = w.WorkExperienceID,
                        JobTitle = w.JobTitle,
                        Company = w.Company,
                        Description = w.Description,
                        StartDate = w.StartDate,
                        EndDate = w.EndDate
                    }).ToList()
                };

                return Results.Created($"/api/persons/{person.ID}", result);
            });

            //app.MapPost("/api/persons", async (CreatePersonWithDetailsDto dto, CvApiDBContext dbContext) =>
            //{
            //    var (isValid, errors) = InputValidator.Validate(dto);
            //    if (!isValid) return Results.BadRequest(errors);

            //    var person = new Person
            //    {
            //        FullName = dto.FullName,
            //        Email = dto.Email,
            //        Phone = dto.Phone,
            //        Description = dto.Description,
            //        Educations = dto.Educations.Select(e => new Education
            //        {
            //            School = e.School,
            //            Degree = e.Degree,
            //            StartDate = e.StartDate,
            //            EndDate = e.EndDate
            //        }).ToList(),
            //        WorkExperiences = dto.WorkExperiences.Select(w => new WorkExperience 
            //        { 
            //            JobTitle = w.JobTitle,
            //            Company = w.Company,
            //            Description = w.Description,
            //            StartDate = w.StartDate,
            //            EndDate = w.EndDate
            //        }).ToList()
            //    };

            //    dbContext.Person.Add(person);
            //    await dbContext.SaveChangesAsync();

            //    var result = new PersonDetailDto
            //    {
            //        FullName = person.FullName,
            //        Email = person.Email,
            //        Phone = person.Phone,
            //        Description = person.Description,
            //        Educations = person.Educations.Select(e => new EducationDTO
            //        {
            //            EducationID = e.EducationID,
            //            School = e.School,
            //            Degree = e.Degree,
            //            StartDate = e.StartDate,
            //            EndDate = e.EndDate
            //        }).ToList(),
            //        WorkExperiences = person.WorkExperiences.Select(w => new WorkExperienceDTO
            //        {
            //            WorkExperienceID =w.WorkExperienceID,
            //            JobTitle = w.JobTitle,
            //            Company = w.Company,
            //            Description = w.Description,
            //            StartDate = w.StartDate,
            //            EndDate = w.EndDate
            //        }).ToList()
            //    };

            //    return Results.Created($"/api/persons/{person.ID}", result);

            //});

            //Get the people in the database
            app.MapGet("/api/persons", async (CvApiDBContext dbContext) =>
            {
                var persons = await dbContext.Person
                    .Include(p => p.Educations)
                    .Include(p => p.WorkExperiences)
                    .ToListAsync();

                var result = persons.Select(p => new PersonDetailDto
                {
                    FullName = p.FullName,
                    Email = p.Email,
                    Phone = p.Phone,
                    Description = p.Description,
                    Educations = p.Educations.Select(e => new EducationDTO
                    {
                        EducationID = e.EducationID,
                        School = e.School,
                        Degree = e.Degree,
                        StartDate = e.StartDate,
                        EndDate = e.EndDate

                    }).ToList(),
                    WorkExperiences = p.WorkExperiences.Select(w => new WorkExperienceDTO
                    { 
                        WorkExperienceID = w.WorkExperienceID,
                        JobTitle = w.JobTitle,
                        Company = w.Company,
                        Description = w.Description,
                        StartDate = w.StartDate,
                        EndDate = w.EndDate

                    }).ToList()
                });
            return Results.Ok(result);
             
            });


            //Get/Retrieve a specific person by ID 
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
                    return Results.NotFound("Entered person Id could not be found");
                }

                var result = new PersonDetailDto
                {
                    FullName = person.FullName,
                    Email = person.Email,
                    Phone = person.Phone,
                    Description = person.Description,
                    Educations = person.Educations.Select(e => new EducationDTO
                    {
                       EducationID = e.EducationID,
                        School = e.School,
                        Degree = e.Degree,
                        StartDate = e.StartDate,
                        EndDate = e.EndDate
                    }).ToList(),
                    WorkExperiences = person.WorkExperiences.Select(w => new WorkExperienceDTO
                    {
                        WorkExperienceID = w.WorkExperienceID,
                        JobTitle = w.JobTitle,
                        Company = w.Company,
                        Description = w.Description,
                        StartDate = w.StartDate,
                        EndDate = w.EndDate
                    }).ToList()
                };
                return Results.Ok(result);
            });

            // Update a person 
            app.MapPut("/api/persons/{id}", async (int id, PersonDto dto, CvApiDBContext dbContext) =>
            {
                var (isValidId, idErrors) = InputValidator.ValidateId(id);
                if (!isValidId) return Results.BadRequest(idErrors);

                var (isValidDto, dtoErrors) = InputValidator.Validate(dto);
                if (!isValidDto) return Results.BadRequest(dtoErrors);

                var person = await dbContext.Person.FindAsync(id);
                if (person == null)
                {
                    return Results.NotFound("Entered person Id could not be found");
                }

                person.FullName = dto.FullName;
                person.Email = dto.Email;
                person.Phone = dto.Phone;
                person.Description = dto.Description;

                await dbContext.SaveChangesAsync();
                return Results.Ok("Person updated successfully");

            });


            //Remove/Delete a person
            app.MapDelete("/api/persons/{id}", async (int id, CvApiDBContext dbContext) =>
            {
                var (isValid, errors) = InputValidator.ValidateId(id);
                if (!isValid) return Results.BadRequest(errors);

                var person = await dbContext.Person
                    .Include(p => p.Educations)
                    .Include(p => p.WorkExperiences)
                    .FirstOrDefaultAsync(p => p.ID == id);

                if (person == null)
                {
                    return Results.NotFound("Entered person Id could not be found");
                }

                dbContext.Person.Remove(person);
                await dbContext.SaveChangesAsync();

                return Results.Ok("Person removed ssuccessfully");

            });


        }
    }
}
