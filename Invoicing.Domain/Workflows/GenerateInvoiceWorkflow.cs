using Invoicing.Domain.Models.Commands;
using Invoicing.Domain.Models.Events;
using Invoicing.Domain.Operations;
using Invoicing.Domain.Repositories;
using static Invoicing.Domain.Models.Entities.Invoice;

namespace Invoicing.Domain.Workflows;

public class GenerateInvoiceWorkflow
{
    public async Task<InvoiceIssuedEvent.IInvoiceIssuedEvent> ExecuteAsync(
        GenerateInvoiceCommand command,
        IInvoiceRepository repository)
    {
        // Check if invoice already exists for this booking (idempotency)
        var exists = await repository.InvoiceExistsForBookingAsync(command.BookingId);
        if (exists)
        {
            return new InvoiceIssuedEvent.InvoiceFailed(
                new[] { $"Invoice already exists for booking {command.BookingId}" });
        }

        var unvalidated = new UnvalidatedInvoice(command);

        IInvoice result = new ValidateInvoiceOperation().Transform(unvalidated);
        result = new CalculateInvoiceOperation().Transform(result);
        result = new IssueInvoiceOperation().Transform(result);

        if (result is IssuedInvoice issued)
        {
            var invoiceId = await repository.SaveInvoiceAsync(issued);
            return result.ToEvent(invoiceId);
        }

        return result.ToEvent();
    }
}
