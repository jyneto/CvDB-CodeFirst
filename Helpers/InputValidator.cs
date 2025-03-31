using CvCodeFirst.Data;
using CvCodeFirst.DTOs;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace CvCodeFirst.Helpers
{

    public static class InputValidator
    {
        public static (bool isValid, List<string> errors) ValidateGitHubUsername(string username)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(username))
            {
                errors.Add("Github username must not be empty or just whitespace");
                return (false, errors);
            }
            return (true, errors);
        }

        //Validate a list of respiratories
        public static (List<GithubReposDTO> validatedRepos, List<string> errors) ValidateGitHubRepositories(List<JsonElement> rawRepositories)
        {
            var validatedRepos = new List<GithubReposDTO>();
            var errors = new List<string>();

            foreach (var respository in rawRepositories)
            {
                var repoName = respository.TryGetProperty("name", out var nameProp) ? nameProp.GetString() : null;
                var repoUrl = respository.TryGetProperty("html_url", out var urlProp) ? urlProp.GetString() : null;

                //Validation logic
                if (string.IsNullOrWhiteSpace(repoName))
                {
                    errors.Add("Repository lacks valid name.");
                    continue;

                }

                if (string.IsNullOrWhiteSpace(repoUrl))
                {
                    errors.Add($"Repository '{repoName ?? "(Unnamed)"}' lacks a valid URL.");
                    continue;
                }

                //optional properties
                var repoLanguage = respository.TryGetProperty("language", out var langProp) ? langProp.GetString() : null;
                var repoDescription = respository.TryGetProperty("description", out var descProp) ? descProp.GetString() : null;

                validatedRepos.Add(new GithubReposDTO
                {
                    Name = repoName,
                    HtmlUrl = repoUrl,
                    Language = repoLanguage,
                    Description = repoDescription

                });
            }
            return (validatedRepos, errors);
        }

        public static (bool isValid, List<string> errors) Validate<T>(T dto)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(dto);

            bool isValid = Validator.TryValidateObject(dto, context, validationResults, true);
            var errors = validationResults.Select(v => v.ErrorMessage ?? "Invalid input").ToList();

            foreach (var prop in typeof(T).GetProperties())
            {
                if (prop.PropertyType == typeof(string))
                {
                    var value = prop.GetValue(dto) as string;
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        errors.Add($"{prop.Name} must not be empty or just whitespace.");
                        isValid = false;
                    }
                }
            }

            return (isValid, errors);
        }

        public static (bool isValid, List<string> errors) ValidateId(int id)
        {
            var errors = new List<string>();
            if (id <= 0)
            {
                errors.Add("ID must be greater than 0.");
                return (false, errors);
            }
            return (true, errors);
        }

        public static async Task<(bool personExists, List<string> personErrors)> ValidatePersonExistsAsync(int personId, CvApiDBContext dbContext)
        {
            var errors = new List<string>();
            var exists = await dbContext.Person.AnyAsync(p => p.ID == personId);
            if (!exists)
            {
                errors.Add("Invalid PersonId: person does not exist.");
                return (false, errors);
            }
            return (true, errors);
        }


    }
}
