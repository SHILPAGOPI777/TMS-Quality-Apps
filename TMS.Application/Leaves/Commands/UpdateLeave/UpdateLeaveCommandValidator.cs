using FluentValidation;

namespace TMS.Application.Leaves.Commands.UpdateLeave
{
    public class UpdateLeaveCommandValidator : AbstractValidator<UpdateleaveCommand>
    {
        public UpdateLeaveCommandValidator()
        {
            RuleFor(i => i.EmployeeId)
                 .NotEmpty();
            RuleFor(i => i.Status)
                .MaximumLength(256);
            RuleFor(i => i.StartDate)
                .NotEmpty();
            RuleFor(i => i.EndDate)
                .NotEmpty();
            RuleFor(i => i.LeaveType)
                .NotEmpty();

        }
    }
}
