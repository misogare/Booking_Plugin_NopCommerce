using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.Widgets.FullCalendar.Domain;

namespace Nop.Plugin.Other.ProductViewTracker.Migrations
{
    [NopMigration("2024/01/10 11:00:00", "Widgets.FullCalendar base schema", MigrationProcessType.Installation)]
    public class SchemaMigration : AutoReversingMigration
    {
        public override void Up()
        {
            Create.TableFor<Appointment>();
        }
    }
}