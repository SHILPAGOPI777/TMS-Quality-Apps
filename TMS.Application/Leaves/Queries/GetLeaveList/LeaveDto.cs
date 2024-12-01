using AutoMapper;
using TMS.Application.Common.Mappings;
using TMS.Domain.Entities;
using System;

namespace TMS.Application.Leaves.Queries.GetLeaveList
{
    public class LeaveDto : IMapFrom<Leave>
    {
        public long LeaveId { get; set; }
        public long EmployeeId { get; set; }

        public string LeaveType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Status { get; set; }
        public string AssigneeName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Leave, LeaveDto>()
                .ForMember(dto => dto.LeaveId, exp => exp.MapFrom(i => i.LeaveId))
                .ForMember(dto => dto.EmployeeId, exp => exp.MapFrom(i => i.EmployeeId))
                .ForMember(dto => dto.LeaveType, exp => exp.MapFrom(i => i.LeaveType))
                .ForMember(dto => dto.AssigneeName, exp => exp.MapFrom(i => i.Assignee.FullName))
                .ForMember(dto => dto.StartDate, exp => exp.MapFrom(i => i.StartDate))
                .ForMember(dto => dto.EndDate, exp => exp.MapFrom(i => i.EndDate));
        }
    }
}
