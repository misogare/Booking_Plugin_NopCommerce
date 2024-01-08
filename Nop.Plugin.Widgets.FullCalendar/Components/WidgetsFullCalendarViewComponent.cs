using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Plugin.Widgets.FullCalendar.Models;
using Nop.Plugin.Widgets.FullCalendar;
using Nop.Services.Configuration;
using Nop.Services.Media;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.NivoSlider.Components
{
    [ViewComponent(Name = "WidgetsFullCalendar")]
    public class WidgetsFullCalendarViewComponent : NopViewComponent
    {
        private readonly IStoreContext _storeContext;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly ISettingService _settingService;
        private readonly IPictureService _pictureService;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;

        public WidgetsFullCalendarViewComponent(IStoreContext storeContext,
            IStaticCacheManager staticCacheManager,
            ISettingService settingService,
            IPictureService pictureService,
            IWebHelper webHelper,
            IWorkContext workContext)
        {
            _storeContext = storeContext;
            _staticCacheManager = staticCacheManager;
            _settingService = settingService;
            _pictureService = pictureService;
            _webHelper = webHelper;
            _workContext = workContext;
        }

        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData = null)
        {
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var fullCalendarSettings = await _settingService.LoadSettingAsync<FullCalendarSettings>(storeScope);
            var currentCustomer = await _workContext.GetCurrentCustomerAsync();
            var contactName = currentCustomer.Username;



            var model = new AppointmentModel
            {
                PublicApiKey = fullCalendarSettings.PublicApiKey,
                CalendarId = fullCalendarSettings.CalendarId,
                ClassName = fullCalendarSettings.ClassName,
                AppointmentDate = DateTime.Now,
                ContactName = contactName,
                WidgetZone = widgetZone,
                StartTime = fullCalendarSettings.StartTime,
                EndTime = fullCalendarSettings.EndTime,
                DaysOfWeekEnabled = String.IsNullOrEmpty(fullCalendarSettings.DaysOfWeekEnabled) ? new int[0] : JsonConvert.DeserializeObject<int[]>(fullCalendarSettings.DaysOfWeekEnabled)
            };

            return View("~/Plugins/Widgets.FullCalendar/Views/PublicInfo.cshtml", model);
        }


    }
}
