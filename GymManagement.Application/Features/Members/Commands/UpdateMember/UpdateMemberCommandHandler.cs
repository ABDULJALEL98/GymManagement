using GymManagement.Application.Common.Models;
using GymManagement.Application.Interfaces;
using MediatR;

namespace GymManagement.Application.Features.Members.Commands.UpdateMember;

public class UpdateMemberCommandHandler : IRequestHandler<UpdateMemberCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateMemberCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        UpdateMemberCommand request,
        CancellationToken cancellationToken)
    {
        var member = await _unitOfWork.Members.GetByIdAsync(request.Id, cancellationToken);

        if (member is null)
        {
            return Result.Failure("Member not found");
        }

        var phoneExists = await _unitOfWork.Members.AnyAsync(
            x => x.PhoneNumber == request.PhoneNumber && x.Id != request.Id,
            cancellationToken);

        if (phoneExists)
        {
            return Result.Failure("Phone number already exists");
        }

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var emailExists = await _unitOfWork.Members.AnyAsync(
                x => x.Email == request.Email && x.Id != request.Id,
                cancellationToken);

            if (emailExists)
            {
                return Result.Failure("Email already exists");
            }
        }

        member.FullName = request.FullName;
        member.PhoneNumber = request.PhoneNumber;
        member.Email = request.Email;
        member.DateOfBirth = request.DateOfBirth;
        member.Address = request.Address;
        member.IsActive = request.IsActive;
        member.UpdatedAtUtc = DateTime.UtcNow;

        _unitOfWork.Members.Update(member);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Member updated successfully");
    }
}