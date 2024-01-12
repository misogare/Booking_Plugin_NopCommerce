using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Widgets.FullCalendar.Domain;
using Nop.Data.Extensions;
using System.Data;

namespace Nop.Plugin.Misc.SimpleLMS.Data.Mapping.Builders
{
    public class AppointmentBuilder : NopEntityBuilder<Appointment>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table.WithColumn(nameof(Appointment.Id))
                .AsInt32().PrimaryKey().Identity()
                .WithColumn(nameof(Appointment.AppointmentDateTimeUTC))
                    .AsDateTime().NotNullable()
                .WithColumn(nameof(Appointment.AppointmentReason))
                    .AsString(500).NotNullable()
                .WithColumn(nameof(Appointment.ContactNumber))
                    .AsString().NotNullable()
                .WithColumn(nameof(Appointment.ContactName))
                    .AsString().NotNullable()
                .WithColumn(nameof(Appointment.CreatedOnUTC))
                    .AsDateTime().NotNullable();
        }
    }
}
