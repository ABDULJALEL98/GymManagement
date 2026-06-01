using GymManagement.Application.Common.Models;
using GymManagement.Application.Interfaces;
using GymManagement.Domain.Entities;
using GymManagement.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Application.Features.Bookings.Commands.CreateBooking;

public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateBookingCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(
        CreateBookingCommand request,
        CancellationToken cancellationToken)
    {
        var member = await _unitOfWork.Members.GetByIdAsync(
            request.MemberId,
            cancellationToken);

        if (member is null)
        {
            return Result<Guid>.Failure("Member not found");
        }

        if (!member.IsActive)
        {
            return Result<Guid>.Failure("Member is not active");
        }

        var gymClass = await _unitOfWork.GymClasses.GetByIdAsync(
            request.GymClassId,
            cancellationToken);

        if (gymClass is null)
        {
            return Result<Guid>.Failure("Gym class not found");
        }

        if (!gymClass.IsActive)
        {
            return Result<Guid>.Failure("Gym class is not active");
        }

        var alreadyBooked = await _unitOfWork.Bookings.AnyAsync(
            x => x.MemberId == request.MemberId
                 && x.GymClassId == request.GymClassId
                 && x.Status != BookingStatus.Cancelled,
            cancellationToken);

        if (alreadyBooked)
        {
            return Result<Guid>.Failure("Member already booked this class");
        }

        var currentBookingsCount = await _unitOfWork.Bookings
            .Query()
            .CountAsync(
                x => x.GymClassId == request.GymClassId
                     && x.Status != BookingStatus.Cancelled,
                cancellationToken);

        if (currentBookingsCount >= gymClass.Capacity)
        {
            return Result<Guid>.Failure("Gym class is full");
        }

        var booking = new Booking
        {
            MemberId = request.MemberId,
            GymClassId = request.GymClassId,
            BookingDate = DateTime.UtcNow,
            Status = BookingStatus.Pending
        };

        await _unitOfWork.Bookings.AddAsync(booking, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(booking.Id, "Booking created successfully");
    }
}