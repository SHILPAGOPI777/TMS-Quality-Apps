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

namespace TMS.Application.Issues.Queries.GetIssueDetail
{
    public class IssueDetailVm : IMapFrom<Issue>
    {
        [HiddenInput(DisplayValue = false)]
        public long Id { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "No title specified")]
        [MinLength(5, ErrorMessage = "The title length is at least 5 characters")]
        [MaxLength(64, ErrorMessage = "The title length is up to 64 characters")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [MinLength(5, ErrorMessage = "The description length is at least 5 characters")]
        [MaxLength(256, ErrorMessage = "The description length is up to 256 characters")]
        public string Description { get; set; }

        [Display(Name = "Status")]
        public IssueStatus Status { get; set; }

        [Display(Name = "Priority")]
        [Required(ErrorMessage = "Priority not specified")]
        public PriorityLevel Priority { get; set; }

        [Display(Name = "Select assignee")]
        [Required(ErrorMessage = "Executor not specified")]
        public long AssigneeId { get; set; }

        [Display(Name = "Select reporter")]
        [Required(ErrorMessage = "Author not specified")]
        public long ReporterId { get; set; }

        public string AssigneeName { get; set; }

        public string ReporterName { get; set; }

        public IList<FrameDto> IssueStatuses =
            Enum.GetValues(typeof(IssueStatus))
            .Cast<IssueStatus>()
            .Select(s => new FrameDto { Value = (int)s, Name = s.ToString() })
            .ToList();

        public IList<FrameDto> IssuePriorityLevels =
            Enum.GetValues(typeof(PriorityLevel))
            .Cast<PriorityLevel>()
            .Select(p => new FrameDto { Value = (int)p, Name = p.ToString() })
            .ToList();

        public virtual void Mapping(Profile profile)
        {
            profile.CreateMap<Issue, IssueDetailVm>()
                .ForMember(vm => vm.Id, exp => exp.MapFrom(i => i.IssueId))
                .ForMember(dto => dto.AssigneeName, exp => exp.MapFrom(i => i.Assignee.FullName))
                .ForMember(dto => dto.ReporterName, exp => exp.MapFrom(i => i.Reporter.FullName));
        }
    }
}
