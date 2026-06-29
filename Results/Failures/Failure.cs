namespace Results.Failures;

public record Failure
{
    public readonly int Code;
    public readonly string Message;
    
    protected Failure(int code, string message)
    {
        Code = code;
        Message = message;
    }

    public static DomainViolation DomainViolation => new ();
    public static InsufficientPermissions InsufficientPermissions => new ();
    public static InvariantViolation InvariantViolation => new ();
    public static NotFound NotFound => new ();
    public static ResourceConflict ResourceConflict => new ();

    public static Failure Create(int code, string message) => new (code, message);
}