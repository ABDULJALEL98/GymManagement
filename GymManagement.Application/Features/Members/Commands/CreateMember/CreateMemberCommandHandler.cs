using GymManagement.Application.Common.Models;
using GymManagement.Application.Interfaces;
using GymManagement.Domain.Entities;
using MediatR;

namespace GymManagement.Application.Features.Members.Commands.CreateMember;

public class CreateMemberCommandHandler : IRequestHandler<CreateMemberCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateMemberCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(
        CreateMemberCommand request,
        CancellationToken cancellationToken)
    {
        var phoneExists = await _unitOfWork.Members.AnyAsync(
            x => x.PhoneNumber == request.PhoneNumber,
            cancellationToken);

        if (phoneExists)
        {
            return Result<Guid>.Failure("Phone number already exists");
        }

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var emailExists = await _unitOfWork.Members.AnyAsync(
                x => x.Email == request.Email,
                cancellationToken);

            if (emailExists)
            {
                return Result<Guid>.Failure("Email already exists");
            }
        }

        var member = new Member
        {
            FullName = request.FullName,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            DateOfBirth = request.DateOfBirth,
            Address = request.Address,
            IsActive = request.IsActive
        };

        await _unitOfWork.Members.AddAsync(member, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(member.Id, "Member created successfully");
    }
}