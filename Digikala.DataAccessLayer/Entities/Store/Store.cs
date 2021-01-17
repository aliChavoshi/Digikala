using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Digikala.DataAccessLayer.Entities.Identity;

namespace Digikala.DataAccessLayer.Entities.Store
{
    public class Store : Commands
    {
        [Key]
        public int UserId { get; set; }

        [Display(Name = "نام فروشگاه")]
        [MaxLength(100, ErrorMessage = " تعداد کارکترهای {0} بیش از حد مجاز است")]
        public string Name { get; set; }

        [Display(Name = "آدرس فروشگاه")]
        [MaxLength(200, ErrorMessage = " تعداد کارکترهای {0} بیش از حد مجاز است")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }


        [Display(Name = "شماره تماس")]
        [MaxLength(50, ErrorMessage = " تعداد کارکترهای {0} بیش از حد مجاز است")]
        [DataType(DataType.PhoneNumber)]
        public string Tel { get; set; }

        [Display(Name = "تصویر")]
        [MaxLength(100, ErrorMessage = " تعداد کارکترهای {0} بیش از حد مجاز است")]
        public string Logo { get; set; }

        #region Relations

        [ForeignKey("UserId")]
        public User User { get; set; }

        #endregion
    }
}