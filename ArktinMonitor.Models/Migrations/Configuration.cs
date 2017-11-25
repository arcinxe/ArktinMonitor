using System.Data.Entity.Migrations;

namespace ArktinMonitor.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ArktinMonitorDataAccess>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "ArktinMonitor.Models.ArktinMonitorContext";
        }

        protected override void Seed(ArktinMonitorDataAccess context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}