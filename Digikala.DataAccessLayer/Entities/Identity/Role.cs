using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digikala.DataAccessLayer.Entities.Identity
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "عنوان نقش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = " تعداد کارکترهای {0} بیش از حد مجاز است")]
        public string Title { get; set; }

        public bool IsDelete { get; set; }

        #region Relations

        [InverseProperty("Role")]
        public List<RolePermission> RolePermissions { get; set; }

        [InverseProperty("Role")]
        public List<User> Users { get; set; }

        #endregion
    }
}