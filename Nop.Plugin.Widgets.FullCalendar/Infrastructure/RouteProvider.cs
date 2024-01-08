using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;


namespace Nop.Plugin.Widgets.FullCalendar.Infrastructure
{
        public partial class RouteProvider : IRouteProvider
        {
            public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
            {
                //PDT
                endpointRouteBuilder.MapControllerRoute("Plugin.Widgets.FullCalendar.List", "Plugins/FullCalendar/List",
                        new
                        {
                            controller = "FullCalendar",
                            action = "List"
                        },
                    new[] { "Nop.Plugin.Widgets.FullCalendar.Controllers" });

            }
            public int Priority => -1;
        }
    
}