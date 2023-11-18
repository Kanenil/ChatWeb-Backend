using System.ComponentModel.DataAnnotations;

namespace ChatWeb.Application.Exceptions;

public class ValidationException : ApplicationException
{
    public List<string> Errors { get; set; } = new List<string>();

    public ValidationException(ValidationResult validationResult)
    {
        Errors.Add(validationResult.ErrorMessage);
    }
}
