using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TMS.Application.Common.Interfaces;
using TMS.Domain.Entities;
using TMS.Domain.Enumerations;
using System;
using Microsoft.EntityFrameworkCore;

namespace TMS.Application.Leaves.Commands.CreateLeave
{
    public class CreateLeaveCommand : IRequest<long>
    {
        public long EmployeeId { get; set; }

        public string LeaveType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Status { get; set; }
        public class CreateLeaveCommandHandler : IRequestHandler<CreateLeaveCommand, long>
        {
            private readonly IApplicationDbContext _context;

            public CreateLeaveCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<long> Handle(CreateLeaveCommand request, CancellationToken cancellationToken)
            {
                var entity = new Leave
                {
                    EmployeeId = request.EmployeeId,
                    LeaveType = request.LeaveType,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    Status = request.Status,
                };

                _context.Leaves.Add(entity);

                
                try
                {
                    // Your EF Core SaveChanges or update logic
                    await _context.SaveChangesAsync(cancellationToken);
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine(ex.InnerException?.Message);
                    throw;
                }
                return entity.LeaveId;
            }
        }
    }
}
