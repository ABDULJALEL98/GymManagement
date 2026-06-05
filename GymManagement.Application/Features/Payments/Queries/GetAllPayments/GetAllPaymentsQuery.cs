using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using GymManagement.Domain.Enums;
using MediatR;

namespace GymManagement.Application.Features.Payments.Queries.GetAllPayments;

public class GetAllPaymentsQuery : PagedRequest, IRequest<Result<PagedResult<PaymentDto>>>
{
    public Guid? MemberId { get; set; }

    public PaymentStatus? Status { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }
}