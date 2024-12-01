using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TMS.Application.Common.Mappings;
using TMS.Application.Common.Models;
using TMS.Domain.Entities;
using TMS.Domain.Enumerations;

namespace TMS.Application.Leaves.Queries.GetLeaveDetail
{
    public class LeaveDetailVm : IMapFrom<Leave>
    {
        [HiddenInput(DisplayValue = false)]
        public long LeaveId { get; set; }

        [Display(Name = "LeaveType")]
        [Required(ErrorMessage = "No type specified")]
        public string LeaveType { get; set; }

        [Display(Name = "Start Date")]
        [Required(ErrorMessage = "No Start Date specified")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [Required(ErrorMessage = "No End Date specified")]
        public DateTime EndDate { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string Status { get; set; }

        public long EmployeeId { get; set; }
        public string AssigneeName { get; set; }

        // Add the Mapping method to configure AutoMapper
        public virtual void Mapping(Profile profile)
        {
            profile.CreateMap<Leave, LeaveDetailVm>()
                .ForMember(vm => vm.LeaveId, exp => exp.MapFrom(l => l.LeaveId))
                .ForMember(vm => vm.LeaveType, exp => exp.MapFrom(l => l.LeaveType))
                .ForMember(vm => vm.StartDate, exp => exp.MapFrom(l => l.StartDate))
                .ForMember(vm => vm.EndDate, exp => exp.MapFrom(l => l.EndDate))
                .ForMember(vm => vm.Status, exp => exp.MapFrom(l => l.Status))
            .ForMember(dto => dto.AssigneeName, exp => exp.MapFrom(i => i.Assignee.FullName));
        }
    }
}
