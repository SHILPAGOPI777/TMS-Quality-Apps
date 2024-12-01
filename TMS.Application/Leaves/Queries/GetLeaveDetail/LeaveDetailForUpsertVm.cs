using AutoMapper;
using System;
using System.Collections.Generic;
using TMS.Application.Common.Models;
using TMS.Domain.Entities;

namespace TMS.Application.Leaves.Queries.GetLeaveDetail
{
    public class LeaveDetailForUpsertVm : LeaveDetailVm
    {
        public override void Mapping(Profile profile)
        {
            profile.CreateMap<Leave, LeaveDetailForUpsertVm>()
                .ForMember(vm => vm.LeaveId, exp => exp.MapFrom(i => i.LeaveId))
                .ForMember(vm => vm.LeaveType, exp => exp.MapFrom(i => i.LeaveType))
                .ForMember(vm => vm.StartDate, exp => exp.MapFrom(i => i.StartDate))
                .ForMember(vm => vm.EndDate, exp => exp.MapFrom(i => i.EndDate))
                .ForMember(dto => dto.AssigneeName, exp => exp.MapFrom(i => i.Assignee.FullName));
        }
    }
}
