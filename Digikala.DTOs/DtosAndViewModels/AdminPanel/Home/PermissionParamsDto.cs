using Digikala.DTOs.FormGenericDto.Public;

namespace Digikala.DTOs.DtosAndViewModels.AdminPanel.Home
{
    public class PermissionParamsDto : PublicFilterFormParams
    {
        public string FilterTitle { get; set; } = "";
        public string FilterRoot { get; set; } = "";
    }
}