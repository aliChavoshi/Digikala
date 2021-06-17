using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digikala.DataAccessLayer.Entities.Identity
{
    public class RolePermission
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "نقش")]
        public int RoleId { get; set; }

        [Required]
        [Display(Name = "دسترسی")]
        public int PermissionId { get; set; }

        [Display(Name = "تاریخ انقضا دسترسی")]
        public DateTime? ExpireRolePermission { get; set; }

        #region Relations
        [ForeignKey("PermissionId")]
        public Permission Permission { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }
        #endregion
    }
}