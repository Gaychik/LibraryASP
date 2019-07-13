namespace LibraryASP.NET.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.History", "BookID");
            AddForeignKey("dbo.History", "BookID", "dbo.Book", "BookID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.History", "BookID", "dbo.Book");
            DropIndex("dbo.History", new[] { "BookID" });
        }
    }
}
