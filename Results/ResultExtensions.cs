using SomeTimeLater.Results.Errors;

namespace SomeTimeLater.Results;

public static class ResultExtensions
{
    extension(Task<Result> resultTask)
    {
        public async Task<Result<T>> ToResultAsync<T>()
        {
            var result = await resultTask;
            return result.AsResult<T>();
        }
    }
    
    extension<T>(Task<Result<T>> resultTask)
    {
        public async Task<Result> AsResultAsync()
        {
            var result = await resultTask;
            return result.AsResult();
        }
        
        public async Task<Result<TOther>> AsResultAsync<TOther>()
        {
            var result = await resultTask;
            return result.AsResult<TOther>();
        }
    }
    
    extension<TResult>(IEnumerable<TResult> results) 
        where TResult : Result
    {
        public bool AllSuccessful()
        {
            return results.All(r => r.IsSuccessful);
        }
        
        public IEnumerable<Error> GetErrors()
        {
            return results.SelectMany(r => r.Errors);
        }
        
        public IEnumerable<string> GetLogMessages()
        {
            return results.SelectMany(r => r.LogMessages);
        }
        
        public bool AnyFailure()
        {
            return results.Any(r => r.IsFailure);
        }
    }
    
    extension(Result[] results)
    {
        public Result Aggregate()
        {
            var isAllSuccessful = results.AllSuccessful();
            return isAllSuccessful
                ? Result.Success()
                : results.CreateFailureResult();
        }

        private Result CreateFailureResult()
        {
            var errors = results.GetErrors();
            var result = Result.Fail(errors.ToList());
            return result;
        }
    }
    
    extension<T>(Result<T>[] results)
    {
        public Result<IEnumerable<T>> Aggregate()
        {
            var isAllSuccessful = results.AllSuccessful();
            return isAllSuccessful
                ? Result.Success(results.GetOutputs())
                : results.CreateFailureResult();
        }

        public IEnumerable<T> GetOutputs()
        {
            return results.Select(r => r.Output);
        }

        private Result<IEnumerable<T>> CreateFailureResult()
        {
            var errors = results.GetErrors();
            var result = Result.Fail<IEnumerable<T>>(errors.ToList());
            return result;
        }
    }
}