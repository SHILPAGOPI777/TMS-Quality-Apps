using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TMS.Application.Common.Interfaces;
using TMS.Application.Leaves.Commands.CreateLeave;
using TMS.Application.Leaves.Queries.GetLeaveDetail;
using TMS.Application.Employees.Queries.GetEmployeeDetail;
using System;
using System.Linq;

namespace TMS.WebUI.Leaves
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public CreateModel(IMediator mediator, ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        public LeaveDetailForUpsertVm Leave { get; set; } = new LeaveDetailForUpsertVm
        {
            StartDate = DateTime.Now,
            EndDate = DateTime.Now
        };

        public async Task OnGetAsync()
        {
            Leave = await _mediator.Send(new GetEmptyLeaveQuery());
            Leave.StartDate = DateTime.Now;
            Leave.EndDate = DateTime.Now;
        }

        public async Task<IActionResult> OnPostAsync(CreateLeaveCommand command)
        {
            var currentEmployee = await _mediator.Send(new GetEmployeeByAppUserIdQuery { AppUserId = _currentUserService.UserId });
            command.EmployeeId = currentEmployee.Id;
            command.Status = "Pending";

            var result = await _mediator.Send(command);
            if (result > 0)
            {
                return RedirectToPage("./Detailed", new { id = result });
            }
            return BadRequest(result);
        }

    }

}
