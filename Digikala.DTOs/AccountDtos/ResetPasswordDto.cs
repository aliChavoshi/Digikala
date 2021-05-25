using System.ComponentModel.DataAnnotations;

namespace Digikala.DTOs.AccountDtos
{
    public class ResetPasswordDto
    {
        [Display(Name = "تلفن همراه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(11, ErrorMessage = " تعداد کارکترهای {0} بیش از حد مجاز است")]
        [MinLength(11, ErrorMessage = "شماره همراه حداقل 11 کاراکتر میباشد.")]
        [DataType(DataType.PhoneNumber)]
        public string Mobile { get; set; }

        [Display(Name = "کلمه عبور جدید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "کلمه عبور نباید از پنج کارکتر کمتر باشد")]
        public string Password { get; set; }

        [Display(Name = "تایید کلمه عبور جدید")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [MinLength(5, ErrorMessage = "کلمه عبور نباید از پنج کارکتر کمتر باشد")]
        [Compare(nameof(Password), ErrorMessage = "کلمه عبور یکسان نمیباشد.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "کد فعال سازی")]
        [MaxLength(5, ErrorMessage = "{0} اشتباه وارد شده است")]
        [MinLength(5, ErrorMessage = "{0} اشتباه وارد شده است")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Password, ErrorMessage = "مقدار عددی وارد کنید کد وارد شده اشتباه است")]
        public string ActiveCode { get; set; }
    }
}