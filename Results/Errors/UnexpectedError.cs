using Results.Failures;

namespace Results.Errors;

public sealed record UnexpectedError : Error
{
    private readonly string? _because;
    
    private UnexpectedError(Failure failure,  string? because = null) 
        : base(failure)
    {
        _because = because; 
    }
    
    public static UnexpectedError InvariantViolation(string? because = null) 
        => new (Failure.InvariantViolation, because);
    
    public static UnexpectedError From(Failure failure, string? because = null) 
        => new (failure, because);
    
    public static UnexpectedError<T> InvariantViolation<T>(string? because = null) 
        => new (Failure.InvariantViolation, because);
    
    public static UnexpectedError<T> From<T>(Failure failure, string? because = null) 
        => new (failure, because);
    
    public override string ToLogMessage()
    {
        var sentenceStarter = $"{Failure.Message}";
        return string.IsNullOrWhiteSpace(_because) 
            ? sentenceStarter 
            : $"{sentenceStarter} because {_because}";
    }
}

public sealed record UnexpectedError<T> : Error<T>
{
    private readonly string? _because;
    
    internal UnexpectedError(Failure failure, string? because = null) 
        : base(failure)
    {
        _because = because; 
    }

    public override string ToLogMessage()
    {
        var sentenceStarter = $"{Failure.Message} while trying to get {typeof(T).Name}";
        return string.IsNullOrWhiteSpace(_because) 
            ? sentenceStarter 
            : $"{sentenceStarter} because {_because}";
    }
}