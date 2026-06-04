using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using MediatR;

namespace GymManagement.Application.Features.Payments.Queries.GetAllPayments;

public class GetAllPaymentsQuery : IRequest<Result<List<PaymentDto>>>
{
}