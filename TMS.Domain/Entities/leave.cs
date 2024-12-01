using TMS.Domain.Common;
using TMS.Domain.Enumerations;
using System;

namespace TMS.Domain.Entities
{
    public class Leave : AuditableEntity
    {
        public long LeaveId { get; set; }

        public long EmployeeId { get; set; }
        public Employee Assignee { get; set; }

        public string LeaveType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Status { get; set; }

      
    }
}
