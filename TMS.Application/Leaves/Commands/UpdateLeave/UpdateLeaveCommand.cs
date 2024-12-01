using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TMS.Application.Common.Exceptions;
using TMS.Application.Common.Interfaces;
using TMS.Domain.Entities;
using TMS.Domain.Enumerations;

namespace TMS.Application.Leaves.Commands.UpdateLeave
{
    public class UpdateleaveCommand : IRequest
    {
        public long LeaveId { get; set; }
        public long EmployeeId { get; set; }

        public string LeaveType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Status { get; set; }

        public class UpdateLeaveCommandHandler : IRequestHandler<UpdateleaveCommand>
        {
            private readonly IApplicationDbContext _context;

            public UpdateLeaveCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(UpdateleaveCommand request, CancellationToken cancellationToken)
            {
                var entity = await _context.Leaves.FindAsync(request.LeaveId);

                if (entity == null)
                {
                    throw new NotFoundException(nameof(Leave), request.LeaveId);
                }

                entity.EmployeeId = request.EmployeeId;
                entity.LeaveType = request.LeaveType;
                entity.StartDate = request.StartDate;
                entity.EndDate = request.EndDate;
                entity.Status = request.Status;

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }               
    }
}
