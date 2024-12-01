using AutoMapper.QueryableExtensions;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using TMS.Application.Common.Interfaces;

namespace TMS.Application.Leaves.Queries.GetLeaveList
{
    public class GetLeaveListQuery : IRequest<LeaveListVm>
    {
        public class GetLeaveListQueryHandler : IRequestHandler<GetLeaveListQuery, LeaveListVm>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetLeaveListQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<LeaveListVm> Handle(GetLeaveListQuery request, CancellationToken cancellationToken)
            {
                var leaves = await _context.Leaves
                    .ProjectTo<LeaveDto>(_mapper.ConfigurationProvider)
                    .OrderBy(p => p.StartDate)
                    .ToListAsync(cancellationToken);

                var vm = new LeaveListVm
                {
                    Leaves = leaves
                };

                return vm;
            }
        }
    }
}
