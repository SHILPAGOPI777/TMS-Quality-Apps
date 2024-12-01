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

namespace TMS.Application.Employees.Queries.GetEmployeeDetail
{
    public class EmployeeDetailVm : IMapFrom<Employee>
    {
        [HiddenInput(DisplayValue = false)]
        public long Id { get; set; }

        public string AppUserId { get; set; }

        [Display(Name = "Short Name")]
        [Required(ErrorMessage = "Short Name is required")]
        public string ShortName { get; set; }

        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Full Name is required")]
        [MinLength(5, ErrorMessage = "Full Name must be at least 5 characters long")]
        [MaxLength(64, ErrorMessage = "Full Name must be at most 64 characters long")]
        public string FullName { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required")]
        [MinLength(5, ErrorMessage = "Email must be at least 5 characters long")]
        [MaxLength(64, ErrorMessage = "Email must be at most 64 characters long")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        [MinLength(5, ErrorMessage = "Password must be at least 5 characters long")]
        [MaxLength(64, ErrorMessage = "Password must be at most 64 characters long")]
        public string Password { get; set; }

        [Display(Name = "Active")]
        [Range(typeof(bool), "true", "true")]
        public bool Active { get; set; }

        [Display(Name = "Select Role")]
        public UserRole Role { get; set; }

        public string RoleName { get; set; }

        public IList<FrameDto> UserRoles =
            Enum.GetValues(typeof(UserRole))
            .Cast<UserRole>()
            .Select(r => new FrameDto { Value = (int)r, Name = r.ToString() })
            .ToList();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Employee, EmployeeDetailVm>()
                .ForMember(d => d.Id, opt => opt.MapFrom(e => e.EmployeeId));
        }
    }
}
