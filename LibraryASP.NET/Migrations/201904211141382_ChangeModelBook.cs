namespace LibraryASP.NET.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeModelBook : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.History", "BookID", "dbo.Book");
            DropIndex("dbo.History", new[] { "BookID" });
            AddColumn("dbo.History", "Person", c => c.String());
            AddColumn("dbo.History", "BookName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.History", "BookName");
            DropColumn("dbo.History", "Person");
            CreateIndex("dbo.History", "BookID");
            AddForeignKey("dbo.History", "BookID", "dbo.Book", "BookID", cascadeDelete: true);
        }
    }
}
