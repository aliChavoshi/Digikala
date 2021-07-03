using Digikala.DTOs.FormGenericDto.Public;

namespace Digikala.DTOs.DtosAndViewModels.AdminPanel.Category
{
    public class CategoryParamsDto: PublicFilterFormParams
    {
        public string FilterTitle { get; set; } = "";
        public string FilterRoot { get; set; } = "";
    }
}