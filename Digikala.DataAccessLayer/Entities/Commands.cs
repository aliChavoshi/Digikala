using System;
using System.ComponentModel.DataAnnotations;

namespace Digikala.DataAccessLayer.Entities
{
    public class Commands
    {
        [Display(Name = "توضیحات")]
        [MaxLength(500, ErrorMessage = "تعداد کارکتر ها بیش از حد مجاز است")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "زمان ثبت")]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        [Display(Name = "زمان ویرایش")]
        public DateTime? ModificationDate { get; set; }

        public bool IsDeleted { get; set; } = false;

        [Display(Name = "وضعیت")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "تعداد ویرایش")]
        public int Version { get; set; } = 0;
    }
}