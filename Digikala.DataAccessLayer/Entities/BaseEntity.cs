using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Digikala.DataAccessLayer.Entities.Identity;

namespace Digikala.DataAccessLayer.Entities
{
    public class BaseEntity
    {
        [Key]
        [Column(Order = 1)]
        public int Id { get; set; }

        #region 8Command

        [Display(Name = "توضیحات")]
        [MaxLength(500, ErrorMessage = "تعداد کارکتر ها بیش از حد مجاز است")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "زمان ثبت")]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        [Display(Name = "زمان ویرایش")]
        public DateTime? ModificationDate { get; set; }

        [Required]
        [Display(Name = "ثبت کننده")]
        public int CreatorUser { get; set; }

        [Display(Name = "ویرایش کننده")]
        public int? ModifierUser { get; set; }

        public bool IsDeleted { get; set; } = false;

        [Display(Name = "وضعیت")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "تعداد ویرایش")]
        public int? Version { get; set; } = 0;

        #endregion

        #region Relations

        [ForeignKey("CreatorUser")]
        public User UserCreator { get; set; }

        [ForeignKey("ModifierUser")]
        public User UserModifier { get; set; }

        #endregion
    }
}