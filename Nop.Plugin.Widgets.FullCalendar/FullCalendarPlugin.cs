using System.Collections.Generic;
using System.IO;
using Nop.Core;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Core.Domain.Messages;
using Nop.Services.Plugins;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;
using Nop.Web.Framework.Infrastructure;
using System.Linq;

namespace Nop.Plugin.Widgets.FullCalendar
{
    /// <summary>
    /// PLugin
    /// </summary>
    public class FullCalendarPlugin : BasePlugin, IWidgetPlugin
    {
        private readonly ILocalizationService _localizationService;
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly IMessageTemplateService _messageTemplateService;
        private readonly IStoreContext _storeContext;

        public FullCalendarPlugin(ILocalizationService localizationService, IPictureService pictureService,
            ISettingService settingService, IWebHelper webHelper,
            IMessageTemplateService messageTemplateService,
            IStoreContext storeContext)
        {
            this._localizationService = localizationService;
            this._pictureService = pictureService;
            this._settingService = settingService;
            this._webHelper = webHelper;
            this._messageTemplateService = messageTemplateService;
            this._storeContext = storeContext;
        }


        /// <summary>
        /// Install plugin
        /// </summary>
        public override async Task InstallAsync()
        {
          

            //create appointment message template
            var messageTemplate = new MessageTemplate
            {
                Name = "Appointment.New",
                Subject = "%Store.Name%: New appointment requested for %Customer.FullName%.",
                Body = "<p><a href=\"%Store.URL%/Admin/Customer/Edit/%Customer.Id%\">%Customer.FullName%</a> has requested a new appointment.</p><dl><dt>Contact Number:</dt><dd>%Appointment.ContactNumber%</dd><dt>Email:</dt><dd>%Customer.Email%</dd><dt>Date:</dt><dd>%Appointment.Date%</dd><dt>Reason:</dt><dd>%Appointment.Reason%</dd></dl>",
                IsActive = true,
                EmailAccountId = 1,
            };
            await _messageTemplateService.InsertMessageTemplateAsync(messageTemplate);

            //settings
            var settings = new FullCalendarSettings
            {
                CalendarId = "38tvhnke4vtaj8u6or8o59out4@group.calendar.google.com",
                PublicApiKey = "AIzaSyB-oGNxrlENnrcBpGtizNOCaoGgH9q4h6Q",
                ClassName = "cal3",
            };

            await _settingService.SaveSettingAsync(settings);

            await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
            {
              
                ["Plugins.Widgets.FullCalendar.Instructions"] = "<h5>Make your Google Calendar public:</h5><ol><li>In the Google Calendar interface, locate the \"My calendars\" area on the left.</li><div>Hover over the calendar you need and click the downward arrow.</li><li>A menu will appear. Click \"Share this Calendar\".</li><li>Check \"Make this calendar public\".</li><li>Make sure \"Share only my free/busy information\" is unchecked.</li><li>Click \"Save\".</li></ol><h5>Obtain your Google Calendar's ID:</h5><ol><li>In the Google Calendar interface, locate the \"My calendars\" area on the left.</li><li>Hover over the calendar you need and click the downward arrow.</li><li>A menu will appear. Click \"Calendar settings\".</li><li>In the \"Calendar Address\" section of the screen, you will see your Calendar ID. It will look something like \"abcd1234@group.calendar.google.com\".</li></ol>",
                ["Plugins.Widgets.FullCalendar.Form.Success"] = "Requested Appointment",
                ["Plugins.Widgets.FullCalendar.Form.Error"] = "Failing adding appointment, please contact your vet.",
                ["Plugins.Widgets.FullCalendar.Form.AppointmentDate"] = "Appointment Date",
                ["Plugins.Widgets.FullCalendar.Form.AppointmentReason"] = "Appointment Reason",
                ["Plugins.Widgets.FullCalendar.Form.AppointmentReasonReq"] = "THIS CANNOT BE NULL",
                ["Plugins.Widgets.FullCalendar.Form.AppointmentDateReq"] = "THIS CANNOT BE NULL",
                ["Plugins.Widgets.FullCalendar.Form.ContactName"] = "Contact Name",
                ["Plugins.Widgets.FullCalendar.Form.ContactNumber"] = "Contact Number",
                ["Plugins.Widgets.FullCalendar.Form.ContactNumberRequired"] = "Contact Number is required",
                ["Plugins.Widgets.FullCalendar.Form.PhoneHelperText"] = "Enter a number where we can contact you.",
                ["Plugins.Widgets.FullCalendar.Form.Title"] = "Request an Appointment",
                ["Plugins.Widgets.FullCalendar.Config.CalendarId"] = "Google Calendar Id",
                ["Plugins.Widgets.FullCalendar.Config.ClassName"] = "Class Name",
                ["Plugins.Widgets.FullCalendar.Config.PublicApiKey"] = "Public Api Key",
                ["Plugins.Widgets.FullCalendar.Config.CreatedOn"] = "Created On",
                ["Plugins.Widgets.FullCalendar.Config.DaysOfWeekEnabled"] = "Days Of Week Enabled",
                ["Plugins.Widgets.FullCalendar.Config.StartTime"] = "Start Time",
                ["Plugins.Widgets.FullCalendar.Config.EndTime"] = "End Time",
                ["Plugins.Widgets.FullCalendar.Config.Google.Header"] = "Google Calendar Settings",
                ["Plugins.Widgets.FullCalendar.Config.DaysOfWeek.Header"] = "Days of Operation",
                ["Plugins.Widgets.FullCalendar.Config.DaysOfWeek.Instructions"] = "Check which days your store is open.",
                ["Plugins.Widgets.FullCalendar.Config.Hours.Header"] = "Hours of Operation",
                ["Plugins.Widgets.FullCalendar.Config.Hours.Instructions"] = "Set which hours your store is open.",
                ["Plugins.Widgets.FullCalendar.Title"] = "Calendar",
                ["Plugins.Widgets.FullCalendar.BodyText"] = "Change body text"
            });

            await base.InstallAsync();

        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override async Task UninstallAsync()
        {

            var store = await _storeContext.GetCurrentStoreAsync();
            var messageTemplates = await _messageTemplateService.GetMessageTemplatesByNameAsync("Appointment.New", store.Id);
            var messageTemplate = messageTemplates.FirstOrDefault();

            if (messageTemplate != null)
               await _messageTemplateService.DeleteMessageTemplateAsync(messageTemplate);
            
            //settings
            await _settingService.DeleteSettingAsync<FullCalendarSettings>();

            //locales
            await _localizationService.DeleteLocaleResourcesAsync("Plugins.Widgets.FullCalendar");


            await base.UninstallAsync();
        }
        public bool HideInWidgetList => false;

        public Task<IList<string>> GetWidgetZonesAsync()
        {
            return Task.FromResult<IList<string>>(new List<string> { PublicWidgetZones.HomepageBeforeNews });

        }

        public string GetWidgetViewComponentName(string widgetZone)
        {
            return "WidgetsFullCalendar";
        }
        public override string GetConfigurationPageUrl()
        {
            return _webHelper.GetStoreLocation() + "Admin/WidgetsFullCalendar/Configure";
        }
    }
}
