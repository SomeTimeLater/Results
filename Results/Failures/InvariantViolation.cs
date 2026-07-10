namespace SomeTimeLater.Results.Failures;

public record InvariantViolation : Failure
{
    internal InvariantViolation() : this("An invariant condition was violated") {}
    protected InvariantViolation(string message) : base(FailureCodes.InternalServerError, message) {}
}