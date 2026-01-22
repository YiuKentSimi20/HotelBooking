using FrontDesk.Domain.Exceptions;
using static FrontDesk.Domain.Models.Entities.CheckIn;

namespace FrontDesk.Domain.Operations;

public abstract class CheckInOperation : DomainOperation<ICheckIn, object, ICheckIn>
{
    internal ICheckIn Transform(ICheckIn checkIn) => Transform(checkIn, null);

    public override ICheckIn Transform(ICheckIn checkIn, object? state) => checkIn switch
    {
        UnvalidatedCheckIn unvalidated => OnUnvalidated(unvalidated),
        ValidatedCheckIn validated => OnValidated(validated),
        AccessCodeGeneratedCheckIn accessCodeGenerated => OnAccessCodeGenerated(accessCodeGenerated),
        CompletedCheckIn completed => OnCompleted(completed),
        InvalidCheckIn invalid => OnInvalid(invalid),
        _ => throw new InvalidCheckInStateException(checkIn.GetType().Name)
    };

    protected virtual ICheckIn OnUnvalidated(UnvalidatedCheckIn checkIn) => checkIn;
    protected virtual ICheckIn OnValidated(ValidatedCheckIn checkIn) => checkIn;
    protected virtual ICheckIn OnAccessCodeGenerated(AccessCodeGeneratedCheckIn checkIn) => checkIn;
    protected virtual ICheckIn OnCompleted(CompletedCheckIn checkIn) => checkIn;
    protected virtual ICheckIn OnInvalid(InvalidCheckIn checkIn) => checkIn;
}
