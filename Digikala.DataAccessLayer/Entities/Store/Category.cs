using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digikala.DataAccessLayer.Entities.Store
{
    public class Category : BaseEntity
    {
        [Display(Name = "زیر مجموعه کدام است ؟")]
        public int? ParentId { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "تعداد کارکتر های {0} بیش از حد مجاز است ")]
        [MinLength(3, ErrorMessage = "تعداد کارکتر های {0} کمتر از حد مجاز است")]
        public string Name { get; set; }

        [Display(Name = "آیکون")]
        [MaxLength(30, ErrorMessage = "تعداد کارکتر های {0} بیش از حد مجاز است ")]
        public string Icon { get; set; }

        #region Relation

        [ForeignKey(nameof(ParentId))]
        public Category Parent { get; set; }

        #endregion
    }
}