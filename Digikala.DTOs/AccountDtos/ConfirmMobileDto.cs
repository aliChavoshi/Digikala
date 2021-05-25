using System.ComponentModel.DataAnnotations;

namespace Digikala.DTOs.AccountDtos
{
    public class ConfirmMobileDto
    {
        [Display(Name = "تلفن همراه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(11, ErrorMessage = "{0} اشتباه وارد شده است")]
        [MinLength(11, ErrorMessage = "{0} اشتباه وارد شده است")]
        public string Mobile { get; set; }

        [Display(Name = "کد فعال سازی")]
        [MaxLength(5, ErrorMessage = "{0} اشتباه وارد شده است")]
        [MinLength(5, ErrorMessage = "{0} اشتباه وارد شده است")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.Password, ErrorMessage = "مقدار عددی وارد کنید کد وارد شده اشتباه است")]
        public int[] ActiveCode { get; set; }
    }
}