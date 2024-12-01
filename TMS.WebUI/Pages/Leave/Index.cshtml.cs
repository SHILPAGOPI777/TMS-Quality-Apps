using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMS.Application.Common.Interfaces;
using TMS.Application.Leaves.Queries.GetLeaveList;
using TMS.Application.Employees.Queries.GetEmployeeDetail;

namespace TMS.WebUI.Leaves
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;
        private LeaveListVm _leaveVm;

        public IndexModel(IMediator mediator, ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        public Dictionary<string, IEnumerable<LeaveDto>> LeavesCatalog { get; private set; }

        public async Task OnGetAsync()
        {
            _leaveVm = await _mediator.Send(new GetLeaveListQuery());

            var AllLeaves = _leaveVm.Leaves;


            LeavesCatalog = new Dictionary<string, IEnumerable<LeaveDto>>
            {
                {"All Leaves:", AllLeaves }
            };
        }
      

    }
}
