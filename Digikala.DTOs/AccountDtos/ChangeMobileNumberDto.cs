using System.ComponentModel.DataAnnotations;

namespace Digikala.DTOs.AccountDtos
{
    public class ChangeMobileNumberDto
    {
        [Display(Name = "تلفن همراه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(11, ErrorMessage = "{0} اشتباه وارد شده است")]
        [MinLength(11, ErrorMessage = "{0} اشتباه وارد شده است")]
        public string OldMobile { get; set; }

        [Display(Name = "تلفن همراه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(11, ErrorMessage = "{0} اشتباه وارد شده است")]
        [MinLength(11, ErrorMessage = "{0} اشتباه وارد شده است")]
        public string NewMobile { get; set; }
    }
}