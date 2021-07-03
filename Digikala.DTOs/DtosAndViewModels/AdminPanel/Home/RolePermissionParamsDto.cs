using Digikala.DTOs.FormGenericDto.Public;

namespace Digikala.DTOs.DtosAndViewModels.AdminPanel.Home
{
    public class RolePermissionParamsDto : PublicFilterFormParams
    {
        public string FilterRole { get; set; }
        public string FilterPermission { get; set; }
    }
}