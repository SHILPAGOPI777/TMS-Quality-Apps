using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TMS.Application.Common.Interfaces;
using TMS.Application.Common.Models;
using System;

namespace TMS.Application.Leaves.Queries.GetLeaveDetail
{
    public class GetEmptyLeaveQuery : IRequest<LeaveDetailForUpsertVm>
    {
        public class GetEmptyLeaveQueryHandler : IRequestHandler<GetEmptyLeaveQuery, LeaveDetailForUpsertVm>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetEmptyLeaveQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<LeaveDetailForUpsertVm> Handle(GetEmptyLeaveQuery request, CancellationToken cancellationToken)
            {
                var employees = await _context.Employees
                    .Select(e => new FrameDto { Value = e.EmployeeId, Name = e.FullName })
                    .ToListAsync(cancellationToken);

                var vm = new LeaveDetailForUpsertVm 
                { 
                    LeaveType = string.Empty, 
                    StartDate = DateTime.MinValue, 
                    EndDate = DateTime.MinValue,
                    Status = string.Empty,                     
                };

                return vm;
            }
        }
    }
}
