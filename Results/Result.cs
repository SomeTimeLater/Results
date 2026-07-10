using System.Diagnostics.CodeAnalysis;
using SomeTimeLater.Results.Errors;
using SomeTimeLater.Results.Failures;

namespace SomeTimeLater.Results;

public class Result
{
    public bool IsSuccessful => !IsFailure;
    public bool IsFailure => 0 < _errors.Count;
    public IReadOnlyCollection<Error> Errors => _errors.AsReadOnly();
    public IEnumerable<string> LogMessages => _errors.ToLogMessages();
    
    private readonly List<Error> _errors = [];

    protected Result() { }

    protected Result(Error error)
    {
        _errors.Add(error);
    }
    
    protected Result(List<Error> errors)
    {
        _errors.AddRange(errors);
    }
    
    public bool IsNotFound => ContainsErrorWith<NotFound>();
    
    public void CombineWith(params Result[] otherResults)
    {
        if (otherResults.AllSuccessful())
            return;
        var errors = otherResults.SelectMany(r => r.Errors).ToArray();
        if (errors.Length == 0)
        {
            return;
        }
        AddErrors(errors);
    }
    
    public bool BothSuccessful(Result otherResult) 
        => IsSuccessful && otherResult.IsSuccessful;

    public bool ContainsErrorWith<TFailure>()
        where TFailure : Failure
        => _errors.Count != 0 && _errors.Contains<TFailure>();

    public void AddErrors(IEnumerable<Error> errors)
      => AddError(errors.ToArray());

    public void AddError(params Error[] errors)
      => _errors.AddRange(errors);

    public Error FirstError()
      => _errors.First();
    
    public IEnumerable<DomainError> GetDomainErrors()
        => GetErrorsOfType<DomainError>();
    
    public IEnumerable<T> GetErrorsOfType<T>()
        where T : Error
        => _errors.OfType<T>();
    
    public IEnumerable<Error> GetErrorsBy<TFailure>()
        where TFailure : Failure
        => _errors.GetErrorsBy<TFailure>();
    
    public IEnumerable<Error<TOutput>> GetErrorsOfOutputType<TOutput>() 
        => _errors.GetErrorsOfOutputType<TOutput>();
    
    public IEnumerable<Error> GetErrorsBy(Func<Error, bool> predicate) => 
        _errors.Where(predicate);

    public virtual Result<T> AsResult<T>()
        => IsSuccessful ? 
            throw new ResultConversionException($"Cannot convert to result {typeof(T).Name} when the result is successful.") 
            : new Result<T>(_errors);
    
    public Task<Result> AsTaskResult()
         => Task.FromResult(this);
    
    public void EnsureSuccess(string? failureMessage = null)
        => EnsureSuccess(() => new ResultFailureException(failureMessage ?? "Result is not successful"));

    public void EnsureSuccess(Func<Exception> exceptionFactory)
    {
        if (IsFailure)
            exceptionFactory();
    }
    
    public static Result Success()
      => new ();
    
    public static Result DomainViolation(string message)
        => new (DomainError.Violation(message));
    
    public static Result InsufficientPermissions(string message)
        => new (DomainError.InsufficientPermissions(message));
    
    public static Result InvariantViolation(string? because = "")
        => new (UnexpectedError.InvariantViolation(because));
    
    public static Result NotFound(string message)
        => new (DomainError.NotFound(message));
    
    public static Result ResourceConflict(string message)
        => new (DomainError.ResourceConflict(message));
    
    public static Result Fail(Failure failure, string? because = null)
        => new (UnexpectedError.From(failure, because));
    
    public static Result Fail(DomainFailure failure, string message)
        => new (DomainError.From(failure, message));
    
    public static Result Fail(Error error)
        => new (error);
    
    public static Result Fail(Error[] errors)
        => Fail(errors.ToList());
    
    public static Result Fail(List<Error> errors)
        => new (errors);
    
    public static Result From(Result result)
        => new (result._errors);
    
    public static Result Combine(params Result[] results)
      => results.Aggregate();
    
    public static Result<T> Success<T>(T output)
        => new (output);
    
    public static Result<T> DomainViolation<T>(string message)
        => new (DomainError.Violation<T>(message));
    
    public static Result<T> InsufficientPermissions<T>(string message)
        => new (DomainError.InsufficientPermissions<T>(message));
    
    public static Result<T> InvariantViolation<T>(string? because = "")
        => new (UnexpectedError.InvariantViolation<T>(because));
    
    public static Result<T> NotFound<T>(string message)
        => new (DomainError.NotFound<T>(message));
    
    public static Result<T> ResourceConflict<T>(string message)
        => new (DomainError.ResourceConflict<T>(message));
    
    public static Result<T> Fail<T>(Failure failure, string? because = null)
        => new (UnexpectedError.From<T>(failure, because));
    
    public static Result<T> Fail<T>(DomainFailure failure, string message)
        => new (DomainError.From<T>(failure, message));
    
    public static Result<T> Fail<T>(Error error)
        => new (error);
    
    public static Result<T> Fail<T>(Error<T> error)
        => new (error);
    
    public static Result<T> Fail<T>(Error[] errors)
        => Fail<T>(errors.ToList());
    
    public static Result<T> Fail<T>(List<Error> errors)
        => new (errors);

    public static Result<T> From<T>(Result result)
        => result.AsResult<T>();
    
    public static Result<IEnumerable<T>> Combine<T>(params Result<T>[] results)
        => results.Aggregate();
    
    public static implicit operator Result(Error error)
        => Fail(error);
    
    public static implicit operator Result(Error[] errors) 
        => new (errors.ToList());
    
    public static implicit operator Result(List<Error> errors) 
        => new (errors);
}

public class Result<T> : Result 
{
    public T Output
    {
        get
        {
            if (_output is not null && HasOutput)
            {
                return _output;
            }
            return !HasOutput 
                ? throw new ResultOutputAccessException($"Cannot access the output of type {typeof(T).Name} when the result is a failure.") 
                : default!;
        }
    }
    
    public bool HasOutput { get; private set; }
    
    public Type OutputType => typeof(T);

    private T? _output;

    internal Result(T output)
    {
        SetOutput(output);
    }

    internal Result(Error error) : base(error) { }
    
    internal Result(List<Error> errors) : base(errors) { }
    
    public bool TryGetOutput([NotNullWhen(true)] out T? output)
    {
        output = _output;
        return HasOutput;
    }

    public void SetOutput(T output)
    {
        _output = output;
        HasOutput = true;
    }
    
    public void RemoveOutput()
    {
        _output = default;
        HasOutput = false;
    }
    
    public T? Unwrap() => _output;
    
    public T GetOrDefault(T defaultValue) 
        =>  HasOutput ?  Output : defaultValue;

    public IEnumerable<Error<T>> GetErrorsOfOutputType()
        => GetErrorsOfOutputType<T>();
    
    public Result AsResult()
        => this;

    public override Result<TOther> AsResult<TOther>()
    {
        if (IsFailure)
        {
            return new Result<TOther>(Errors.ToList());
        }
        if (Output is TOther output)
        {
            return new Result<TOther>(output);
        }
        throw new ResultConversionException($"Error converting {typeof(T).Name} to {typeof(TOther).Name}");
    }
    
    public Task<Result<TOther>> AsTaskResult<TOther>()
    {
        return Task.FromResult(AsResult<TOther>());
    }

    public static implicit operator T(Result<T> result) 
        => result.Output;
    
    public static implicit operator Result<T>(T output) 
        => new (output);
    
    public static implicit operator Result<T>(Error error) 
        => new (error);
    
    public static implicit operator Result<T>(Error[] errors) 
        => new (errors.ToList());
    
    public static implicit operator Result<T>(List<Error> errors) 
        => new (errors);
}