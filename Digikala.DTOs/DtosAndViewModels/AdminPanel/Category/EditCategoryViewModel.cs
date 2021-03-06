﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Digikala.DTOs.DtosAndViewModels.AdminPanel.Category
{
    public class EditCategoryViewModel
    {
        public int Id { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "تعداد کارکتر های {0} بیش از حد مجاز است ")]
        [MinLength(3, ErrorMessage = "تعداد کارکتر های {0} کمتر از حد مجاز است")]
        public string Name { get; set; }

        [Display(Name = "آیکون")]
        public string Icon { get; set; }

        [Display(Name = "توضیحات")]
        [MaxLength(500, ErrorMessage = "تعداد کارکتر ها بیش از حد مجاز است")]
        public string Description { get; set; }

        public int Version { get; set; }
    }
}