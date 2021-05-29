using System.ComponentModel.DataAnnotations;

namespace Digikala.DTOs.Store
{
    public class LoginStoreDto
    {
        [Display(Name = "ایمیل ")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "کلمه عبور نباید از پنج کارکتر کمتر باشد")]
        public string Password { get; set; }
    }
}