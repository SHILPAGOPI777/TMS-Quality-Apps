using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TMS.Application.Leaves.Commands.CreateLeave;
using TMS.Application.Leaves.Commands.DeleteLeave;
using TMS.Application.Leaves.Commands.UpdateLeave;
using TMS.Application.Leaves.Queries.GetLeaveDetail;
using TMS.Application.Leaves.Queries.GetLeaveList;

namespace TMS.WebUI.Controllers
{
    [Authorize]
    public class LeaveController : ApiController
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<LeaveListVm>> GetAll()
        {
            return Ok(await Mediator.Send(new GetLeaveListQuery()));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<LeaveDetailVm>> Get(long id)
        {
            return Ok(await Mediator.Send(new GetLeaveDetailQuery { LeaveId = id }));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Create(CreateLeaveCommand command)
        {
            var result = await Mediator.Send(command);

            if (result > 0)
            {
                return Ok(result);
            }

            return NoContent();
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Update(long id, UpdateleaveCommand command)
        {
            if (id != command.LeaveId)
            {
                return BadRequest();
            }

            await Mediator.Send(command);

            return NoContent();
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
        {
            await Mediator.Send(new DeleteLeaveCommand { Id = id });

            return NoContent();
        }
    }
}
