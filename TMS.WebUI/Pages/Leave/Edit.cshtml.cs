using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using TMS.Application.Leaves.Commands.DeleteLeave;
using TMS.Application.Leaves.Commands.UpdateLeave;
using TMS.Application.Leaves.Queries.GetLeaveDetail;

namespace TMS.WebUI.Leaves
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IMediator _mediator;

        public EditModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public LeaveDetailForUpsertVm Leave { get; set; }

        public async Task OnGetAsync(long id)
        {
            Leave = await _mediator.Send(new GetLeaveDetailForEditQuery { LeaveId = id });
        }

        public async Task<IActionResult> OnPostAsync(long id)
        {
            if (!ModelState.IsValid)
            {
                return Page(); // Return the same page with validation errors
            }

            if (id != Leave.LeaveId)
            {
                return BadRequest();
            }

            var command = new UpdateleaveCommand
            {
                LeaveId = Leave.LeaveId,
                LeaveType = Leave.LeaveType,
                StartDate = Leave.StartDate,
                EndDate = Leave.EndDate,
                EmployeeId = Leave.EmployeeId,
                Status = "Approved" // Set status directly
            };

            await _mediator.Send(command);

            return RedirectToPage("./Detailed", new { id = Leave.LeaveId });
        }

        public async Task<IActionResult> OnPostDeleteAsync(long id)
        {
            await _mediator.Send(new DeleteLeaveCommand { Id = id });

            return RedirectToPage("./Index");
        }
    }

}
