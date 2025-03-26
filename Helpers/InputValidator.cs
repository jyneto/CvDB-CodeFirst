using CvCodeFirst.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.Xml;
using System.Security.Principal;

namespace CvCodeFirst.Helpers
{

    public static class InputValidator
    {
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

        public static async Task<(bool isValid, List<string> errors)> ValidatePersonExistsAsync(int personId, CvApiDBContext dbContext)
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
