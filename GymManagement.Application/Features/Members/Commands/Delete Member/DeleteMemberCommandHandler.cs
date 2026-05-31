using GymManagement.Application.Common.Models;
using GymManagement.Application.Interfaces;
using MediatR;

namespace GymManagement.Application.Features.Members.Commands.DeleteMember;

public class DeleteMemberCommandHandler : IRequestHandler<DeleteMemberCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteMemberCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        DeleteMemberCommand request,
        CancellationToken cancellationToken)
    {
        var member = await _unitOfWork.Members.GetByIdAsync(request.Id, cancellationToken);

        if (member is null)
        {
            return Result.Failure("Member not found");
        }

        _unitOfWork.Members.Delete(member);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Member deleted successfully");
    }
}