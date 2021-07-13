using System.ComponentModel.DataAnnotations;
using Digikala.Utility.Validations;
using Microsoft.AspNetCore.Http;

namespace Digikala.DTOs.Store
{
    public class PropertiesStoreDto
    {
        public int UserId { get; set; }

        [Display(Name = "نام فروشگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = " تعداد کارکترهای {0} بیش از حد مجاز است")]
        public string Name { get; set; }

        [Display(Name = "آدرس فروشگاه")]
        [MaxLength(200, ErrorMessage = " تعداد کارکترهای {0} بیش از حد مجاز است")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Address { get; set; }

        [Display(Name = "شماره تماس")]
        [MaxLength(50, ErrorMessage = " تعداد کارکترهای {0} بیش از حد مجاز است")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Tel { get; set; }

        [Display(Name = "تصویر")]
        [FileSizeValidator(2)]
        [ContentTypeValidator(ContentTypeValidator.ContentTypeGroup.Image)]
        public IFormFile Logo { get; set; }

        public string OldLogo { get; set; }

        [Display(Name = "توضیحات")]
        [MaxLength(500, ErrorMessage = "تعداد کارکتر ها بیش از حد مجاز است")]
        public string Description { get; set; }

        public int Version { get; set; } = 0;
    }
}