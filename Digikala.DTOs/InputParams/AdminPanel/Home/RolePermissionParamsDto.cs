using Digikala.DTOs.FormDto.Public;

namespace Digikala.DTOs.InputParams.AdminPanel.Home
{
    public class RolePermissionParamsDto : PublicFilterFormParams
    {
        public string FilterRole { get; set; }
        public string FilterPermission { get; set; }
    }
}