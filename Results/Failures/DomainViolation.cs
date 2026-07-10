namespace SomeTimeLater.Results.Failures;

public record DomainViolation : DomainFailure
{
    internal DomainViolation() : this( "A domain violation occurred") {}
    protected DomainViolation(string message) : base(FailureCodes.BadRequest, message) {}
}