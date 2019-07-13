namespace LibraryASP.NET.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeModelBook1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.History", "DateReceipt", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.History", "DateReceipt", c => c.DateTime(nullable: false));
        }
    }
}
