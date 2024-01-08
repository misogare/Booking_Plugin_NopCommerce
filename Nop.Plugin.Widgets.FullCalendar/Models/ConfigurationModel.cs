using Nop.Web.Framework;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.ModelBinding;
using System;

namespace Nop.Plugin.Widgets.FullCalendar.Models
{
    public record ConfigurationModel : BaseNopModel
    {
        [NopResourceDisplayName("Plugins.Widgets.FullCalendar.Config.CalendarId")]
        public string CalendarId { get; set; } 
        public bool CalenderId_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.FullCalendar.Config.ClassName")]
        public string ClassName { get; set; }

        public bool ClassName_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.FullCalendar.Config.PublicApiKey")]
        public string PublicApiKey { get; set; }
        public bool PublicApiKey_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.FullCalendar.Config.DaysOfWeekEnabled")]
        public int[] DaysOfWeekEnabled { get; set; }
        public bool DaysOfWeekEnabled_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.FullCalendar.Config.StartTime")]
        public TimeSpan StartTime { get; set; }
        public bool StartTime_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.FullCalendar.Config.EndTime")]
        public TimeSpan EndTime { get; set; }
        public bool EndTime_OverrideForStore { get; set; }

    }
}
