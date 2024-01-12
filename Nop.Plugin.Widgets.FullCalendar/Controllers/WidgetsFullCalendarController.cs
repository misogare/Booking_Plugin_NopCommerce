using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Orders;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework;
using Nop.Plugin.Widgets.FullCalendar.Models;
using Nop.Plugin.Widgets.FullCalendar.Services;
using Nop.Services.Helpers;
using Nop.Plugin.Widgets.FullCalendar.Domain;
using Newtonsoft.Json;
using Nop.Web.Framework.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Nop.Services.Security;
using System.Threading.Tasks;
using Nop.Services.Messages;
using Microsoft.AspNetCore.Authorization;

namespace Nop.Plugin.Widgets.FullCalendar.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    [AutoValidateAntiforgeryToken]
    public class WidgetsFullCalendarController : BasePluginController
    {
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IStoreService _storeService;
        private readonly ISettingService _settingService;
        private readonly IOrderService _orderService;
        private readonly ILogger _logger;
        private readonly ICategoryService _categoryService;
        private readonly IProductAttributeParser _productAttributeParser;
        private readonly ILocalizationService _localizationService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IAppointmentService _appointmentService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IPermissionService _permissionService;
        private readonly INotificationService _notificationService;


        public WidgetsFullCalendarController(IWorkContext workContext,
            IStoreContext storeContext,
            IStoreService storeService,
            ISettingService settingService,
            IOrderService orderService,
            ILogger logger,
            ICategoryService categoryService,
            IProductAttributeParser productAttributeParser,
            ILocalizationService localizationService,
            IGenericAttributeService genericAttributeService,
            IAppointmentService appointmentService,
            IDateTimeHelper datetimeHelper,
            IPermissionService permissionService,
            INotificationService notificationService
            )

        {
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._storeService = storeService;
            this._settingService = settingService;
            this._orderService = orderService;
            this._logger = logger;
            this._categoryService = categoryService;
            this._productAttributeParser = productAttributeParser;
            this._localizationService = localizationService;
            this._genericAttributeService = genericAttributeService;
            this._appointmentService = appointmentService;
            this._dateTimeHelper = datetimeHelper;
            this._permissionService = permissionService;
            this._notificationService = notificationService;
        }
        [AuthorizeAdmin]
        public async Task<IActionResult> Configure()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            // Load settings for a chosen store scope
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var fullCalendarSettings = await _settingService.LoadSettingAsync<FullCalendarSettings>(storeScope);

            var model = new ConfigurationModel
            {
                PublicApiKey = fullCalendarSettings.PublicApiKey,
                CalendarId = fullCalendarSettings.CalendarId,
                ClassName = fullCalendarSettings.ClassName,
                DaysOfWeekEnabled = String.IsNullOrEmpty(fullCalendarSettings.DaysOfWeekEnabled) ? new int[0] : JsonConvert.DeserializeObject<int[]>(fullCalendarSettings.DaysOfWeekEnabled),
                StartTime = fullCalendarSettings.StartTime,
                EndTime = fullCalendarSettings.EndTime
            };

            if (storeScope > 0)
            {
                // Assuming _settingService.SettingExistsAsync is asynchronous
                model.PublicApiKey_OverrideForStore = await _settingService.SettingExistsAsync(fullCalendarSettings, x => x.PublicApiKey, storeScope);
                model.CalenderId_OverrideForStore = await _settingService.SettingExistsAsync(fullCalendarSettings, x => x.CalendarId, storeScope);
                model.ClassName_OverrideForStore = await _settingService.SettingExistsAsync(fullCalendarSettings, x => x.ClassName, storeScope);
                model.DaysOfWeekEnabled_OverrideForStore = await _settingService.SettingExistsAsync(fullCalendarSettings, x => x.DaysOfWeekEnabled, storeScope);
                model.StartTime_OverrideForStore = await _settingService.SettingExistsAsync(fullCalendarSettings, x => x.StartTime, storeScope);
                model.EndTime_OverrideForStore = await _settingService.SettingExistsAsync(fullCalendarSettings, x => x.EndTime, storeScope);

                // Add other settings checks
            }
           
            return View("~/Plugins/Widgets.FullCalendar/Views/Configure.cshtml", model);
        }


        [HttpPost]
        
        public async Task<IActionResult> Configure(ConfigurationModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            // Load settings for a chosen store scope
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var fullCalendarSettings = await _settingService.LoadSettingAsync<FullCalendarSettings>(storeScope);

            fullCalendarSettings.PublicApiKey = model.PublicApiKey;
            fullCalendarSettings.CalendarId = model.CalendarId;
            fullCalendarSettings.ClassName = model.ClassName;
            fullCalendarSettings.DaysOfWeekEnabled = JsonConvert.SerializeObject(model.DaysOfWeekEnabled);
            fullCalendarSettings.StartTime = model.StartTime;
            fullCalendarSettings.EndTime = model.EndTime;

            await _settingService.SaveSettingAsync(fullCalendarSettings);


            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            await _settingService.SaveSettingOverridablePerStoreAsync(fullCalendarSettings, x => x.PublicApiKey, model.PublicApiKey_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(fullCalendarSettings, x => x.CalendarId, model.CalenderId_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(fullCalendarSettings, x => x.ClassName, model.ClassName_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(fullCalendarSettings, x => x.DaysOfWeekEnabled, model.DaysOfWeekEnabled_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(fullCalendarSettings, x => x.StartTime, model.StartTime_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(fullCalendarSettings, x => x.EndTime, model.EndTime_OverrideForStore, storeScope, false);

            await _settingService.ClearCacheAsync();

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

            return await Configure();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Plugins/Widgets.FullCalendar/PublicInfo")]
        public async Task<IActionResult> PublicInfo(PublicInfoModel model)
        {
            if (ModelState.IsValid) // check form inputs
            {
                try
                {
                    var appointment = new Appointment
                    {
                        AppointmentDateTimeUTC = _dateTimeHelper.ConvertToUtcTime(model.AppointmentDate),
                        AppointmentReason = model.AppointmentReason,
                        ContactName = model.ContactName,
                        ContactNumber = model.ContactNumber,
                        CreatedOnUTC = DateTime.UtcNow,
                    };

                    // create new appointment
                    await _appointmentService.InsertAppointment(appointment);

                    // log result for debugging
                    var logMessage = $"New appointment created on {model.AppointmentDate} for {model.ContactNumber}";
                    await _logger.InformationAsync(logMessage);

                    // show success notification
                    _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

                    // return
                    return View("~/Plugins/Widgets.FullCalendar/Views/PublicInfo.cshtml", model);


                }
                catch (Exception ex)
                {
                    // log error message
                 
                    await _logger.ErrorAsync("Error creating appointment request.", ex, await _workContext.GetCurrentCustomerAsync());


                    // show error notification
                    _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.error"));

                    // return
                    return View("~/Plugins/Widgets.FullCalendar/Views/PublicInfo.cshtml", model);
                }
            }

            return View("~/Plugins/Widgets.FullCalendar/Views/PublicInfo.cshtml", model);
        }
        [HttpPost]
        [Route("Plugins/Widgets.FullCalendar/List")]
        public virtual async Task<IActionResult> List()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();
            var appointments = await _appointmentService.GetAllAppointments(true);
            var data = appointments.SelectAwait(async x => new PublicInfoModel
            {
                AppointmentDate = await _dateTimeHelper.ConvertToUserTimeAsync(x.AppointmentDateTimeUTC, DateTimeKind.Utc),
                AppointmentReason = x.AppointmentReason,
                ContactName = x.ContactName,
                ContactNumber = x.ContactNumber,
                Created = await _dateTimeHelper.ConvertToUserTimeAsync(x.CreatedOnUTC, DateTimeKind.Utc),
            });

            return Json(new { data = data });
        }

    }
}