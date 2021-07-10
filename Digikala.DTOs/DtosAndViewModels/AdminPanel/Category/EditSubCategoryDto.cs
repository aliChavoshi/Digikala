using System.ComponentModel.DataAnnotations;

namespace Digikala.DTOs.DtosAndViewModels.AdminPanel.Category
{
    public class EditSubCategoryDto
    {
        public int Id { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "تعداد کارکتر های {0} بیش از حد مجاز است ")]
        [MinLength(3, ErrorMessage = "تعداد کارکتر های {0} کمتر از حد مجاز است")]
        public string Name { get; set; }

        public int RootId { get; set; }
        public int Version { get; set; }
    }
}