using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TMS.Application.Common.Exceptions;
using TMS.Application.Common.Interfaces;
using TMS.Domain.Entities;

namespace TMS.Application.Leaves.Commands.DeleteLeave
{
    public class DeleteLeaveCommand : IRequest
    {
        public long Id { get; set; }

        public class DeleteLeaveCommandHandler : IRequestHandler<DeleteLeaveCommand>
        {
            private readonly IApplicationDbContext _context;

            public DeleteLeaveCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(DeleteLeaveCommand request, CancellationToken cancellationToken)
            {
                var entity = await _context.Issues.FindAsync(request.Id);

                if (entity == null)
                {
                    throw new NotFoundException(nameof(Employee), request.Id);
                }

                _context.Issues.Remove(entity);

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
