using System.Net.Mail;
using System.Text.RegularExpressions;
using FluentValidation;
using LinqToDB.DataProvider;
using Nop.Data;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Widgets.FullCalendar.Models
{
    public class PublicInfoModelvalidator : BaseNopValidator<PublicInfoModel>
    {
        public PublicInfoModelvalidator( ILocalizationService LocalizationService) {

            RuleFor(x => x.ContactNumber).NotEmpty().WithMessageAwait(LocalizationService.GetResourceAsync("Plugins.Widgets.FullCalendar.Form.ContactNumber"));

            RuleFor(x => x.AppointmentReason).NotEmpty().WithMessageAwait(LocalizationService.GetResourceAsync("Plugins.Widgets.FullCalendar.Form.AppointmentReasonReq"));

            RuleFor(x => x.AppointmentDate).NotEmpty().WithMessageAwait(LocalizationService.GetResourceAsync("Plugins.Widgets.FullCalendar.Form.AppointmentDateReq"));

        }
    }
}