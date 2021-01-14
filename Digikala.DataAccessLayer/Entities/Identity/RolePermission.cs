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
        public int RoleId { get; set; }

        [Required]
        public int PermissionId { get; set; }

        public DateTime? ExpireRolePermission { get; set; }
        
        #region Relations
        [ForeignKey("PermissionId")]
        public Permission Permission { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }
        #endregion
    }
}