using GymManagement.Application.Common.Models;
using GymManagement.Application.DTOs;
using GymManagement.Application.Interfaces;
using GymManagement.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Application.Features.Dashboard.Queries.GetDashboardStats;

public class GetDashboardStatsQueryHandler
    : IRequestHandler<GetDashboardStatsQuery, Result<DashboardStatsDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetDashboardStatsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<DashboardStatsDto>> Handle(
        GetDashboardStatsQuery request,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var totalMembers = await _unitOfWork.Members
            .Query()
            .AsNoTracking()
            .CountAsync(cancellationToken);

        var activeMembers = await _unitOfWork.Members
            .Query()
            .AsNoTracking()
            .CountAsync(x => x.IsActive, cancellationToken);

        var totalTrainers = await _unitOfWork.Trainers
            .Query()
            .AsNoTracking()
            .CountAsync(cancellationToken);

        var activeTrainers = await _unitOfWork.Trainers
            .Query()
            .AsNoTracking()
            .CountAsync(x => x.IsActive, cancellationToken);

        var upcomingGymClasses = await _unitOfWork.GymClasses
            .Query()
            .AsNoTracking()
            .CountAsync(x => x.IsActive && x.StartTime >= now, cancellationToken);

        var activeSubscriptions = await _unitOfWork.Subscriptions
            .Query()
            .AsNoTracking()
            .CountAsync(
                x => x.Status == SubscriptionStatus.Active &&
                     x.EndDate >= now,
                cancellationToken);

        var totalBookings = await _unitOfWork.Bookings
            .Query()
            .AsNoTracking()
            .CountAsync(cancellationToken);

        var totalPaidAmount = await _unitOfWork.Payments
            .Query()
            .AsNoTracking()
            .Where(x => x.Status == PaymentStatus.Paid)
            .SumAsync(x => x.Amount, cancellationToken);

        var recentBookings = await _unitOfWork.Bookings
            .Query()
            .AsNoTracking()
            .Include(x => x.Member)
            .Include(x => x.GymClass)
            .OrderByDescending(x => x.BookingDate)
            .Take(5)
            .Select(x => new RecentBookingDto
            {
                Id = x.Id,
                MemberName = x.Member.FullName,
                GymClassName = x.GymClass.Name,
                BookingDate = x.BookingDate,
                Status = x.Status
            })
            .ToListAsync(cancellationToken);

        var recentPayments = await _unitOfWork.Payments
            .Query()
            .AsNoTracking()
            .Include(x => x.Member)
            .OrderByDescending(x => x.PaymentDate)
            .Take(5)
            .Select(x => new RecentPaymentDto
            {
                Id = x.Id,
                MemberName = x.Member.FullName,
                Amount = x.Amount,
                PaymentMethod = x.PaymentMethod,
                PaymentDate = x.PaymentDate,
                Status = x.Status
            })
            .ToListAsync(cancellationToken);

        var dashboard = new DashboardStatsDto
        {
            TotalMembers = totalMembers,
            ActiveMembers = activeMembers,
            TotalTrainers = totalTrainers,
            ActiveTrainers = activeTrainers,
            UpcomingGymClasses = upcomingGymClasses,
            ActiveSubscriptions = activeSubscriptions,
            TotalBookings = totalBookings,
            TotalPaidAmount = totalPaidAmount,
            RecentBookings = recentBookings,
            RecentPayments = recentPayments
        };

        return Result<DashboardStatsDto>.Success(dashboard);
    }
}