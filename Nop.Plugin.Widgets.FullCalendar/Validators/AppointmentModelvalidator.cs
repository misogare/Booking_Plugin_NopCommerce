using System.Net.Mail;
using System.Text.RegularExpressions;
using FluentValidation;
using LinqToDB.DataProvider;
using Nop.Data;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Widgets.FullCalendar.Models
{
    public class AppointmentModelvalidator : BaseNopValidator<AppointmentModel>
    {
        public AppointmentModelvalidator( ILocalizationService LocalizationService, INopDataProvider dataProvider) {

            RuleFor(x => x.ContactNumber).NotEmpty().WithMessage(LocalizationService.GetResource("Plugins.Widgets.FullCalendar.Form.ContactNumber"));

            RuleFor(x => x.AppointmentReason).NotEmpty().WithMessage(LocalizationService.GetResource("Plugins.Widgets.FullCalendar.Form.AppointmentReasonReq"));

            RuleFor(x => x.AppointmentDate).NotEmpty().WithMessage(LocalizationService.GetResource("Plugins.Widgets.FullCalendar.Form.AppointmentDateReq"));

            SetDatabaseValidationRules<AppointmentModel>(dataProvider);
        }
    }
}