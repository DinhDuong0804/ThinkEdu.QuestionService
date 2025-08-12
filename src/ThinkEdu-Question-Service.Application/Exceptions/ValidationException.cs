using FluentValidation.Results;

namespace ThinkEdu_Question_Service.Application.Exceptions
{
    public class ValidationException : ApplicationException
    {
  
        public List<string> ValidationErrors { get; set; }

        public ValidationException(List<ValidationFailure> failures)
        {
            ValidationErrors = failures.Select(f => $"{f.PropertyName}@{f.ErrorMessage}").ToList();
        }
    }
}