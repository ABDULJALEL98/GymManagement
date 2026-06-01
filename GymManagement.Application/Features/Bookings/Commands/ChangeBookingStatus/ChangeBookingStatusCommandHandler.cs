using GymManagement.Application.Common.Models;
using GymManagement.Application.Interfaces;
using GymManagement.Domain.Enums;
using MediatR;

namespace GymManagement.Application.Features.Bookings.Commands.ChangeBookingStatus;

public class ChangeBookingStatusCommandHandler
    : IRequestHandler<ChangeBookingStatusCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public ChangeBookingStatusCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        ChangeBookingStatusCommand request,
        CancellationToken cancellationToken)
    {
        var booking = await _unitOfWork.Bookings.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (booking is null)
        {
            return Result.Failure("Booking not found");
        }

        if (booking.Status == BookingStatus.Cancelled)
        {
            return Result.Failure("Cancelled booking cannot be changed");
        }

        if (booking.Status == BookingStatus.Completed)
        {
            return Result.Failure("Completed booking cannot be changed");
        }

        booking.Status = request.Status;
        booking.UpdatedAtUtc = DateTime.UtcNow;

        _unitOfWork.Bookings.Update(booking);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Booking status updated successfully");
    }
}