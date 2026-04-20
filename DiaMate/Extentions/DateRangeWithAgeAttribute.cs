using System;
using System.ComponentModel.DataAnnotations;

public class DateRangeWithAgeAttribute : ValidationAttribute
{
    private readonly int _minAge;
    private readonly int _maxAge;

    public DateRangeWithAgeAttribute(int minAge, int maxAge)
    {
        _minAge = minAge; 
        _maxAge = maxAge; 
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
            return ValidationResult.Success;

        DateTime inputDate = (DateTime)value;

        DateTime maxDate = DateTime.Now.AddYears(-_minAge); 
        DateTime minDate = DateTime.Now.AddYears(-_maxAge); 

        if (inputDate < minDate || inputDate > maxDate)
        {
            return new ValidationResult(
                $"Date must be between {minDate:yyyy-MM-dd} and {maxDate:yyyy-MM-dd}."
            );
        }

        return ValidationResult.Success;
    }
}