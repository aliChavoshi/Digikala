using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace Digikala.Utility.TagHelper
{
    public static class Helper
    {
        public static string IsActive(this IHtmlHelper helper, string area, string controller, string action)
        {
            ViewContext context = helper.ViewContext;

            RouteValueDictionary values = context.RouteData.Values;

            string _area = values["area"]?.ToString();
            string _controller = values["controller"].ToString();
            string _action = values["action"].ToString();

            if ((action == _action) && (controller == _controller) && (area == _area))
            {
                return "menu-item-active";
            }
            else if ((action == _action) && (controller == _controller))
            {
                return "menu-item-active";
            }
            else
            {
                return "";
            }
        }
    }
}