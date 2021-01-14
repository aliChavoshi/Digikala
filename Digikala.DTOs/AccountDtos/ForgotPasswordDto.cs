using System.ComponentModel.DataAnnotations;

namespace Digikala.DTOs.AccountDtos
{
    public class ForgotPasswordDto
    {
        [Display(Name = "تلفن همراه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(11, ErrorMessage = " تعداد کارکترهای {0} بیش از حد مجاز است")]
        [DataType(DataType.PhoneNumber)]
        [MinLength(11, ErrorMessage = "شماره همراه حداقل 11 کاراکتر میباشد.")]
        public string Mobile { get; set; }
    }
}