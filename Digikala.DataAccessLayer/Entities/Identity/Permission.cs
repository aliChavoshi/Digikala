using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digikala.DataAccessLayer.Entities.Identity
{
    public class Permission
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "عنوان دسترسی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "نمیتواند {0} بیشتر از {1} کاراکتر باشد")]
        public string Name { get; set; }

        [Display(Name = "Root")]
        public int? ParentId { get; set; }

        public bool IsDeleted { get; set; } = false;

        #region Relations

        [InverseProperty("Permission")]
        public List<RolePermission> RolePermissions { get; set; }

        [ForeignKey("ParentId")]
        public List<Permission> Permissions { get; set; }

        #endregion
    }
}