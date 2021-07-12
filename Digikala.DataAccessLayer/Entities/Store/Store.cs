using System;
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

        [Display(Name = "رای مثبت")]
        [Range(0,Int32.MaxValue, ErrorMessage = "لطفا {0} را انتخاب کنید")]
        public int VotePositive { get; set; } = 0;

        [Display(Name = "رای منفی")]
        [Range(0, Int32.MaxValue, ErrorMessage = "لطفا {0} را انتخاب کنید")]
        public int VoteNegative { get; set; } = 0;

        [Display(Name = "تعداد بازگشتی")]
        [Range(0, Int32.MaxValue, ErrorMessage = "لطفا {0} را انتخاب کنید")]
        public int ReturnNumber { get; set; } = 0;

        #region Relations

        [ForeignKey("UserId")]
        public User User { get; set; }

        #endregion
    }
}