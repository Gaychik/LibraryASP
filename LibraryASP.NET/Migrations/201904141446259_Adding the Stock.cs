namespace LibraryASP.NET.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingtheStock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Book", "Stock", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Book", "Stock");
        }
    }
}
