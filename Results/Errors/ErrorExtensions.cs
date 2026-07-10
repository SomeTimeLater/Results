using SomeTimeLater.Results.Failures;

namespace SomeTimeLater.Results.Errors;

public static class ErrorExtensions
{
    extension (IEnumerable<Error> errors)
    {
        public IEnumerable<string> ToLogMessages()
            => errors.Select(e => e.ToLogMessage());
        
        public bool Contains<TFailure>()
            where TFailure : Failure
            => errors.Any(e => e.IsFailureType<TFailure>());
        
        public IEnumerable<Error> GetErrorsBy<TFailure>()
            where TFailure : Failure
            => errors.Where(e => e.IsFailureType<TFailure>());
        
        public IEnumerable<Error<TOutput>> GetErrorsOfOutputType<TOutput>() 
            => errors.OfType<Error<TOutput>>();
    }
}