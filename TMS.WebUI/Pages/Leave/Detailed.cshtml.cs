using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TMS.Application.Leaves.Queries.GetLeaveDetail;

namespace TMS.WebUI.Leaves
{
    [Authorize]
    public class DetailedModel : PageModel
    {
        private readonly IMediator _mediator;

        public DetailedModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public LeaveDetailVm Leave { get; private set; }

        public async Task OnGetAsync(long id)
        {
            Leave = await _mediator.Send(new GetLeaveDetailQuery { LeaveId = id });
        }
    }
}
