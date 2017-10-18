namespace ArktinMonitor.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BlockedSites",
                c => new
                    {
                        BlockedSiteId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        UrlAddress = c.String(),
                        ComputerUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BlockedSiteId)
                .ForeignKey("dbo.ComputerUsers", t => t.ComputerUserId, cascadeDelete: true)
                .Index(t => t.ComputerUserId);
            
            CreateTable(
                "dbo.ComputerUsers",
                c => new
                    {
                        ComputerUserId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FullName = c.String(),
                        PrivilegeLevel = c.String(),
                        Hidden = c.Boolean(nullable: false),
                        ComputerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ComputerUserId)
                .ForeignKey("dbo.Computers", t => t.ComputerId, cascadeDelete: true)
                .Index(t => t.ComputerId);
            
            CreateTable(
                "dbo.Computers",
                c => new
                    {
                        ComputerId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Cpu = c.String(),
                        Gpu = c.String(),
                        Ram = c.Double(nullable: false),
                        OperatingSystem = c.String(),
                        WebAccountId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ComputerId)
                .ForeignKey("dbo.WebAccounts", t => t.WebAccountId, cascadeDelete: true)
                .Index(t => t.WebAccountId);
            
            CreateTable(
                "dbo.WebAccounts",
                c => new
                    {
                        WebAccountId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.WebAccountId);
            
            CreateTable(
                "dbo.BlockedApplications",
                c => new
                    {
                        BlockedApplicationId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Path = c.String(),
                        ComputerUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BlockedApplicationId)
                .ForeignKey("dbo.ComputerUsers", t => t.ComputerUserId, cascadeDelete: true)
                .Index(t => t.ComputerUserId);
            
            CreateTable(
                "dbo.Disks",
                c => new
                    {
                        DiskId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        TotalSpace = c.Double(nullable: false),
                        UsedSpace = c.Double(nullable: false),
                        ComputerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DiskId)
                .ForeignKey("dbo.Computers", t => t.ComputerId, cascadeDelete: true)
                .Index(t => t.ComputerId);
            
            CreateTable(
                "dbo.LogTimeIntervals",
                c => new
                    {
                        LogTimeIntervalId = c.String(nullable: false, maxLength: 128),
                        StartTime = c.DateTime(nullable: false),
                        Duration = c.Time(nullable: false, precision: 7),
                        State = c.String(),
                        ComputerUserId = c.Int(),
                        ComputerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LogTimeIntervalId)
                .ForeignKey("dbo.Computers", t => t.ComputerId, cascadeDelete: true)
                .ForeignKey("dbo.ComputerUsers", t => t.ComputerUserId)
                .Index(t => t.ComputerUserId)
                .Index(t => t.ComputerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogTimeIntervals", "ComputerUserId", "dbo.ComputerUsers");
            DropForeignKey("dbo.LogTimeIntervals", "ComputerId", "dbo.Computers");
            DropForeignKey("dbo.Disks", "ComputerId", "dbo.Computers");
            DropForeignKey("dbo.BlockedApplications", "ComputerUserId", "dbo.ComputerUsers");
            DropForeignKey("dbo.BlockedSites", "ComputerUserId", "dbo.ComputerUsers");
            DropForeignKey("dbo.ComputerUsers", "ComputerId", "dbo.Computers");
            DropForeignKey("dbo.Computers", "WebAccountId", "dbo.WebAccounts");
            DropIndex("dbo.LogTimeIntervals", new[] { "ComputerId" });
            DropIndex("dbo.LogTimeIntervals", new[] { "ComputerUserId" });
            DropIndex("dbo.Disks", new[] { "ComputerId" });
            DropIndex("dbo.BlockedApplications", new[] { "ComputerUserId" });
            DropIndex("dbo.Computers", new[] { "WebAccountId" });
            DropIndex("dbo.ComputerUsers", new[] { "ComputerId" });
            DropIndex("dbo.BlockedSites", new[] { "ComputerUserId" });
            DropTable("dbo.LogTimeIntervals");
            DropTable("dbo.Disks");
            DropTable("dbo.BlockedApplications");
            DropTable("dbo.WebAccounts");
            DropTable("dbo.Computers");
            DropTable("dbo.ComputerUsers");
            DropTable("dbo.BlockedSites");
        }
    }
}
