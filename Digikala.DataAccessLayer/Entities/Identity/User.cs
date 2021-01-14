﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digikala.DataAccessLayer.Entities.Identity
{
    public class User
    {
        [Key]
        [Column(Order = 1)]
        public int Id { get; set; }

        [Display(Name = "نقش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int RoleId { get; set; }

        [Display(Name = "تلفن همراه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(11, ErrorMessage = " تعداد کارکترهای {0} بیش از حد مجاز است")]
        public string Mobile { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Password { get; set; }

        [Display(Name = "ایمیل")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
        public string Email { get; set; }

        [Display(Name = "کد ملی")]
        [MaxLength(10, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string NationalCode { get; set; }

        [Display(Name = "کد فعال سازی")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ActiveCode { get; set; }

        [Display(Name = "نام و نام خانوادگی")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Fullname { get; set; }


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
        public bool IsActive { get; set; } = false;

        [Display(Name = "تعداد ویرایش")]
        public int Version { get; set; } = 0;

        #endregion

        #region Relations

        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        #endregion
    }
}