using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Digikala.Utility.Validations
{
    public class ContentTypeValidator : ValidationAttribute
    {
        //اعتبار سنجی کلی برای همه
        private readonly string[] _validContentTypes;


        //موارد قابل قبول برای عکس این ها میباشد
        private readonly string[] _imageContentTypes = new string[] { "image/jpeg", "image/png", "image/gif" };
        //موارد قابل قبول برای فیلم
        private readonly string[] _videoContentTypes = new string[] { "video/mp4", "video/gpeg" };

        public ContentTypeValidator(string[] validContentTypes)
        {
            _validContentTypes = validContentTypes;
        }

        //این را من بهش میگم که Image  باشه  یا Video
        public ContentTypeValidator(ContentTypeGroup contentTypeGroup)
        {
            switch (contentTypeGroup)
            {
                case ContentTypeGroup.Image:
                    _validContentTypes = _imageContentTypes;
                    break;

                case ContentTypeGroup.Video:
                    _validContentTypes = _videoContentTypes;
                    break;
            }
        }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            //جنسش را مشخص میکنم که از جنس چی هست
            if (!(value is IFormFile formFile))
            {
                return ValidationResult.Success;

            }
            //اگر پسوند فایل شامل موارد ذکر شده نبود بهش خطا بده
            if (!_validContentTypes.Contains(formFile.ContentType))
            {
                return new ValidationResult(
                    $"فرمت های عکس به صورت روبرو میباشد :  {string.Join(", ", _validContentTypes)}");
            }

            return ValidationResult.Success;
        }

        //مواردی که من میتونم انتخاب کنم
        public enum ContentTypeGroup
        {
            Image,
            Video
        }
    }
}