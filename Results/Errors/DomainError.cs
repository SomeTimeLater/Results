using SomeTimeLater.Results.Failures;

namespace SomeTimeLater.Results.Errors;

public record DomainError : Error
{
    public string ErrorCode { get; }
    public string Message { get; }
    public string? PropertyName { get; set; }
    
    private DomainError(DomainFailure failure, string errorCode, string message, string? propertyName = null) 
        : base(failure)
    {
        ErrorCode = errorCode;
        Message = message;
        PropertyName = propertyName;
    }
    
    public static DomainError Violation(string errorCode, string message, string? propertyName = null) 
        => new (Failure.DomainViolation, errorCode, message, propertyName);
    
    public static DomainError InsufficientPermissions(string errorCode, string message, string? propertyName = null) 
        => new (Failure.InsufficientPermissions, errorCode, message, propertyName);
    
    public static DomainError NotFound(string errorCode, string message, string? propertyName = null) 
        => new (Failure.NotFound, errorCode, message, propertyName);
    
    public static DomainError ResourceConflict(string errorCode, string message, string? propertyName = null) 
        => new (Failure.ResourceConflict, errorCode, message, propertyName);
    
    public static DomainError From(DomainFailure failure, string errorCode, string message, string? propertyName = null) 
        => new (failure, errorCode, message, propertyName);
    
    public static DomainError<T> Violation<T>(string errorCode, string message, string? propertyName = null) 
        => new (Failure.DomainViolation, errorCode, message, propertyName);
    
    public static DomainError<T> InsufficientPermissions<T>(string errorCode, string message, string? propertyName = null) 
        => new (Failure.InsufficientPermissions, errorCode, message, propertyName);
    
    public static DomainError<T> NotFound<T>(string errorCode, string message, string? propertyName = null) 
        => new (Failure.NotFound, errorCode, message, propertyName);
    
    public static DomainError<T> ResourceConflict<T>(string errorCode, string message, string? propertyName = null) 
        => new (Failure.ResourceConflict, errorCode, message, propertyName);
    
    public static DomainError<T> From<T>(DomainFailure failure, string errorCode, string message, string? propertyName = null) 
        => new (failure, errorCode, message, propertyName);

    public override string ToLogMessage()
    {
        var sentence = $"{Failure.Message}: {Message}";
        return sentence;
    }
}

public sealed record DomainError<T> : Error<T>
{
    public string ErrorCode { get; }
    public string Message { get; }
    public string? PropertyName { get; set; }
    
    internal DomainError(DomainFailure failure, string errorCode, string message, string? propertyName = null) 
        : base(failure)
    {
        ErrorCode = errorCode;
        Message = message;
    }

    public override string ToLogMessage()
    {
        var sentence = $"{Failure.Message} for {typeof(T).Name}: {Message}";
        return sentence;
    }
    
}