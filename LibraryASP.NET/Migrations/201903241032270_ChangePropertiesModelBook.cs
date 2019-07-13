namespace LibraryASP.NET.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangePropertiesModelBook : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Book", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Book", "Author", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Book", "Author", c => c.String());
            AlterColumn("dbo.Book", "Name", c => c.String());
        }
    }
}
