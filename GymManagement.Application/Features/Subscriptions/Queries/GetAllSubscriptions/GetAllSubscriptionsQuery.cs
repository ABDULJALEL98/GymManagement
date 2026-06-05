using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using GymManagement.Domain.Enums;
using MediatR;

namespace GymManagement.Application.Features.Subscriptions.Queries.GetAllSubscriptions;

public class GetAllSubscriptionsQuery : PagedRequest, IRequest<Result<PagedResult<SubscriptionDto>>>
{
    public Guid? MemberId { get; set; }

    public Guid? SubscriptionPlanId { get; set; }

    public SubscriptionStatus? Status { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }
}