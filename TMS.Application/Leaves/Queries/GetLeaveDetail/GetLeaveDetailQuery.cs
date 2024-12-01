using AutoMapper.QueryableExtensions;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using TMS.Application.Common.Exceptions;
using TMS.Application.Common.Interfaces;
using TMS.Domain.Entities;

namespace TMS.Application.Leaves.Queries.GetLeaveDetail
{
    public class GetLeaveDetailQuery : IRequest<LeaveDetailVm>
    {
        public long LeaveId { get; set; }

        public class GetLeaveDetailQueryHandler : IRequestHandler<GetLeaveDetailQuery, LeaveDetailVm>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetLeaveDetailQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<LeaveDetailVm> Handle(GetLeaveDetailQuery request, CancellationToken cancellationToken)
            {
                var vm = await _context.Leaves
                    .ProjectTo<LeaveDetailVm>(_mapper.ConfigurationProvider)
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
