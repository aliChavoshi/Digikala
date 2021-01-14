using System.ComponentModel.DataAnnotations;

namespace Digikala.DTOs.AccountDtos
{
    public class LoginDto
    {
        [Display(Name = "تلفن همراه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(11, ErrorMessage = " تعداد کارکترهای {0} بیش از حد مجاز است")]
        [DataType(DataType.PhoneNumber)]
        [MinLength(11, ErrorMessage = "شماره همراه حداقل 11 کاراکتر میباشد.")]
        public string Mobile { get; set; }


        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "کلمه عبور نباید از پنج کارکتر کمتر باشد")]
        public string Password { get; set; }

        [Display(Name = "مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }
    }
}