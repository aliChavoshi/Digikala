﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Digikala.DTOs.DtosAndViewModels.AdminPanel.Home
{
    public class CreateRolePermissionDto
    {
        [Display(Name = "نقش :")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Range(1, Int32.MaxValue, ErrorMessage = "لطفا {0} را انتخاب کنید")]
        public int RoleId { get; set; }

        [Display(Name = "دسترسی :")]
        public List<int> PermissionId { get; set; } = new List<int>();

        [Display(Name = "تاریخ انقضای دسترسی :")]
        public List<string> ExpireRolePermission { get; set; } = new List<string>();
    }
}