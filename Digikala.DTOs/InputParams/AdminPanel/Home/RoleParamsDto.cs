using Digikala.DTOs.FormDto.AdminPanel;
using Digikala.DTOs.FormDto.Public;

namespace Digikala.DTOs.InputParams.AdminPanel.Home
{
    public class RoleParamsDto : PublicFilterFormParams
    {
        public string FilterTitle { get; set; } = "";
    }
}