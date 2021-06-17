using System;
using System.ComponentModel.DataAnnotations;

namespace Digikala.DTOs.InputParams.AdminPanel.Home
{
    public class CreateRolePermissionDto
    {
        [Display(Name = "نقش :")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Range(1, Int32.MaxValue, ErrorMessage = "لطفا {0} را انتخاب کنید")]
        public int RoleId { get; set; }

        [Display(Name = "دسترسی :")]
        public int[] PermissionId { get; set; }

        [Display(Name = "تاریخ انقضای دسترسی :")]
        public string[] ExpireRolePermission { get; set; }
    }
}