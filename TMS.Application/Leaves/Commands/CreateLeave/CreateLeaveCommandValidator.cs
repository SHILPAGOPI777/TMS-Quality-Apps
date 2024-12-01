using FluentValidation;

namespace TMS.Application.Leaves.Commands.CreateLeave
{
    public class CreateLeaveCommandValidator : AbstractValidator<CreateLeaveCommand>
    {
        public CreateLeaveCommandValidator()
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
