using Digikala.DTOs.FormDto.Public;

namespace Digikala.DTOs.InputParams.AdminPanel.Category
{
    public class CategoryParamsDto: PublicFilterFormParams
    {
        public string FilterTitle { get; set; } = "";
        public string FilterRoot { get; set; } = "";
    }
}