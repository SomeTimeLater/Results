using Results.Failures;

namespace Results.Errors;

public record DomainError : Error
{
    public string Message { get; }
    
    private DomainError(DomainFailure domainViolation, string message) 
        : base(domainViolation)
    {
        Message = message;
    }
    
    public static DomainError Violation(string message) 
        => new (Failure.DomainViolation, message);
    
    public static DomainError InsufficientPermissions(string message) 
        => new (Failure.InsufficientPermissions, message);
    
    public static DomainError NotFound(string message) 
        => new (Failure.NotFound, message);
    
    public static DomainError ResourceConflict(string message) 
        => new (Failure.ResourceConflict, message);
    
    public static DomainError From(DomainFailure failure, string message) 
        => new (failure, message);
    
    public static DomainError<T> Violation<T>(string message) 
        => new (Failure.DomainViolation, message);
    
    public static DomainError<T> InsufficientPermissions<T>(string message) 
        => new (Failure.InsufficientPermissions, message);
    
    public static DomainError<T> NotFound<T>(string message) 
        => new (Failure.NotFound, message);
    
    public static DomainError<T> ResourceConflict<T>(string message) 
        => new (Failure.ResourceConflict, message);
    
    public static DomainError<T> From<T>(DomainFailure failure, string message) 
        => new (failure, message);

    public override string ToLogMessage()
    {
        var sentence = $"{Failure.Message}: {Message}";
        return sentence;
    }
}

public sealed record DomainError<T> : Error<T>
{
    public string Message { get; }
    
    internal DomainError(DomainFailure domainViolation, string message) 
        : base(domainViolation)
    {
        Message = message;
    }

    public override string ToLogMessage()
    {
        var sentence = $"{Failure.Message} for {typeof(T).Name}: {Message}";
        return sentence;
    }
    
}