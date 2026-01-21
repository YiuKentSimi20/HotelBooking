using Invoicing.Domain.Exceptions;
using static Invoicing.Domain.Models.Entities.Invoice;

namespace Invoicing.Domain.Operations;

public abstract class InvoiceOperation : DomainOperation<IInvoice, object, IInvoice>
{
    internal IInvoice Transform(IInvoice invoice) => Transform(invoice, null);

    public override IInvoice Transform(IInvoice invoice, object? state) => invoice switch
    {
        UnvalidatedInvoice unvalidated => OnUnvalidated(unvalidated),
        ValidatedInvoice validated => OnValidated(validated),
        CalculatedInvoice calculated => OnCalculated(calculated),
        IssuedInvoice issued => OnIssued(issued),
        InvalidInvoice invalid => OnInvalid(invalid),
        _ => throw new InvalidInvoiceStateException(invoice.GetType().Name)
    };

    protected virtual IInvoice OnUnvalidated(UnvalidatedInvoice invoice) => invoice;
    protected virtual IInvoice OnValidated(ValidatedInvoice invoice) => invoice;
    protected virtual IInvoice OnCalculated(CalculatedInvoice invoice) => invoice;
    protected virtual IInvoice OnIssued(IssuedInvoice invoice) => invoice;
    protected virtual IInvoice OnInvalid(InvalidInvoice invoice) => invoice;
}
