using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Validations
{
    public class UppercaseLetterAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }
            var firstLetter = value.ToString()[0].ToString();

            if (firstLetter != firstLetter.ToUpper())
            {
                return new ValidationResult("The first letter is not in Uppercase");
            }
            return ValidationResult.Success;
        }
    }
}
