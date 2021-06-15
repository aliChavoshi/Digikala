using Digikala.DTOs.FormDto.Public;

namespace Digikala.DTOs.InputParams.AdminPanel.Home
{
    public class PermissionParamsDto : PublicFilterFormParams
    {
        public string FilterTitle { get; set; } = "";
        public string FilterRoot { get; set; } = "";
    }
}