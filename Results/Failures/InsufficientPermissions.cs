namespace Results.Failures;

public record InsufficientPermissions : DomainFailure
{
    internal InsufficientPermissions() : this("Insufficient permissions") {}
    protected InsufficientPermissions(string message) : base(FailureCodes.Forbidden, message) {}

}