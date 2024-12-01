using AutoMapper.QueryableExtensions;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using TMS.Application.Common.Exceptions;
using TMS.Application.Common.Interfaces;
using TMS.Application.Common.Models;
using TMS.Domain.Entities;

namespace TMS.Application.Leaves.Queries.GetLeaveDetail
{
    public class GetLeaveDetailForEditQuery : IRequest<LeaveDetailForUpsertVm>
    {
        public long LeaveId { get; set; }

        public class GetLeaveDetailForEditQueryHandler : IRequestHandler<GetLeaveDetailForEditQuery, LeaveDetailForUpsertVm>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetLeaveDetailForEditQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<LeaveDetailForUpsertVm> Handle(GetLeaveDetailForEditQuery request, CancellationToken cancellationToken)
            {
                var vm = await _context.Leaves
                    .ProjectTo<LeaveDetailForUpsertVm>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(p => p.LeaveId == request.LeaveId, cancellationToken);


                if (vm == null)
                {
                    throw new NotFoundException(nameof(Leave), request.LeaveId);
                }

                return vm;
            }
        }
    }
}
