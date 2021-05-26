using System.ComponentModel.DataAnnotations;

namespace Digikala.DTOs.Store
{
    public class StoreRegisterDto
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


        [Display(Name = "ایمیل")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
        [DataType(DataType.EmailAddress)]
        public string StoreEmail { get; set; }
    }
}