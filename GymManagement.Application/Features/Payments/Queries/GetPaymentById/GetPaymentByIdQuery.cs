using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using MediatR;

namespace GymManagement.Application.Features.Payments.Queries.GetPaymentById;

public class GetPaymentByIdQuery : IRequest<Result<PaymentDto>>
{
    public Guid Id { get; set; }

    public GetPaymentByIdQuery(Guid id)
    {
        Id = id;
    }
}