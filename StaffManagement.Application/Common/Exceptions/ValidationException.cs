namespace StaffManagement.Application.Common.Exceptions
{
    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; set; }

        public ValidationException() : base() 
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IEnumerable<FluentValidation.Results.ValidationFailure> failure) : this()
        {
            Errors = failure
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failuregroup => failuregroup.Key, failuregroup => failuregroup.ToArray());
        }
    }
}
