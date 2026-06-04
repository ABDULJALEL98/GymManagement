using GymManagement.Application.Common.Models;
using MediatR;

namespace GymManagement.Application.Features.Payments.Commands.DeletePayment;

public class DeletePaymentCommand : IRequest<Result>
{
    public Guid Id { get; set; }

    public DeletePaymentCommand(Guid id)
    {
        Id = id;
    }
}