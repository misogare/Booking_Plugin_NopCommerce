using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widgets.FullCalendar.Models
{
    public record PublicInfoModel : BaseNopModel
    {
        [NopResourceDisplayName("Plugins.Widgets.FullCalendar.Form.AppointmentDate")]
        public DateTime AppointmentDate { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.FullCalendar.Form.AppointmentReason")]
        public string AppointmentReason { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.FullCalendar.Form.ContactNumber")]
        public string ContactNumber { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.FullCalendar.Form.ContactName")]
        public string ContactName { get; set; }
        public int[] DaysOfWeekEnabled { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string WidgetZone { get; set; }
        public string CalendarId { get; set; }
        public string ClassName { get; set; }
        public string PublicApiKey { get; set; }
        public DateTime Created { get; set; }
    }
}
