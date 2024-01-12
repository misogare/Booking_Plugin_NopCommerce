using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc.Routing;


namespace Nop.Plugin.Widgets.FullCalendar.Infrastructure
{
        public partial class RouteProvider : IRouteProvider
        {
        public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
        {
            //PDT
            endpointRouteBuilder.MapControllerRoute("Plugin.Widgets.FullCalendar.List", "Plugins/WidgetsFullCalendar/List",
                    new
                    {
                        controller = "FullCalendar",
                        action = "List"
                    });
           
        }
            public int Priority => -1;
        }
    
}