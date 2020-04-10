using System;
using System.ComponentModel.DataAnnotations;

namespace CompanyName.ProjectName.WebApi.Attributes.Validation
{
    public class Id : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return Convert.ToInt32(value) > 0 ?
                ValidationResult.Success :
                new ValidationResult($"{validationContext.DisplayName} must have a value greater than 0.");
        }
    }
}
