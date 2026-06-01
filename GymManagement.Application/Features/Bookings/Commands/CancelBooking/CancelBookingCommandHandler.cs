using GymManagement.Application.Common.Models;
using GymManagement.Application.Interfaces;
using GymManagement.Domain.Enums;
using MediatR;

namespace GymManagement.Application.Features.Bookings.Commands.CancelBooking;

public class CancelBookingCommandHandler : IRequestHandler<CancelBookingCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public CancelBookingCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        CancelBookingCommand request,
        CancellationToken cancellationToken)
    {
        var booking = await _unitOfWork.Bookings.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (booking is null)
        {
            return Result.Failure("Booking not found");
        }

        if (booking.Status == BookingStatus.Completed)
        {
            return Result.Failure("Completed booking cannot be cancelled");
        }

        booking.Status = BookingStatus.Cancelled;
        booking.UpdatedAtUtc = DateTime.UtcNow;

        _unitOfWork.Bookings.Update(booking);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Booking cancelled successfully");
    }
}