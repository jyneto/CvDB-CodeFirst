using System.ComponentModel.DataAnnotations;

namespace CvCodeFirst.Helpers
{
    public class NotPlaceholderAttribute : ValidationAttribute
    {
        private readonly string _placeholder;
        public NotPlaceholderAttribute(string placeholder)
        {
            _placeholder = placeholder;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string str && str.Trim().ToLower() == _placeholder.Trim().ToLower())
            {
                return new ValidationResult($"{validationContext.DisplayName} cannot be the default placeholder.");
            }

            return ValidationResult.Success;
        }
    }
    
}
