using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Digikala.Utility.Generator
{
    public interface ISaveFileDirectory
    {
        Task<string> SaveFile(IFormFile file, string path);
        Task<string> DeleteAndSaveFile(string oldFile, IFormFile newFile, string path);
    }

    public class SaveFileDirectory : ISaveFileDirectory
    {
        /// <summary>
        /// این برای زمانی است که کاربر فقط میخواهی فایلی را ایجاد کند
        /// </summary>
        /// <param name="file">فایل جدید</param>
        /// <param name="path">مسیری که میخواهد ذخیره کند</param>
        /// <returns></returns>
        public async Task<string> SaveFile(IFormFile file, string path)
        {
            if (file != null)
            {
                var fileNameWithType = CodeGenerators.GuidId() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), path, fileNameWithType);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return fileNameWithType;
            }
            return null;
        }

        /// <summary>
        /// این برای زمانی است که کاربر میخواهد ویرایش انجام دهد 
        /// </summary>
        /// <param name="oldFile">نام فایل قبلی</param>
        /// <param name="newFile">فایل جدید</param>
        /// <param name="path">مسیری که میخواد ویرایش انجام شود</param>
        /// <returns></returns>
        public async Task<string> DeleteAndSaveFile(string oldFile, IFormFile newFile, string path)
        {
            if (oldFile == null && newFile == null)
            {
                return null;
            }
            else
            {
                //فایل جدیدی انتخاب نکردم ولی فایل قبلی وجود داشته
                if (newFile == null && oldFile != null)
                {
                    return oldFile;
                }
                //فایل جدید انتخاب کردم و فایل قبلی وجود نداشته
                if (newFile != null && oldFile == null)
                {
                    return await SaveFile(newFile, path);
                }
                //فایل قبلی وجود داشته و منم یک فایل جدید بهش اضافه کردم
                if (newFile != null && oldFile != null)
                {
                    string deletePath = Path.Combine(Directory.GetCurrentDirectory(), path, oldFile);
                    if (File.Exists(deletePath))
                    {
                        File.Delete(deletePath);
                    }
                    return await SaveFile(newFile, path);
                }
            }
            return null;
        }
    }
}
