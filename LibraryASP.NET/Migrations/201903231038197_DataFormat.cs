namespace LibraryASP.NET.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataFormat : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Person", "FirstName", c => c.String(nullable: false));
            AlterColumn("dbo.Person", "LastName", c => c.String(nullable: false));
            AlterColumn("dbo.Person", "MidName", c => c.String(nullable: false));
            AlterColumn("dbo.Person", "Address", c => c.String(nullable: false));
            AlterColumn("dbo.Person", "TelephoneNumber", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Person", "TelephoneNumber", c => c.String());
            AlterColumn("dbo.Person", "Address", c => c.String());
            AlterColumn("dbo.Person", "MidName", c => c.String());
            AlterColumn("dbo.Person", "LastName", c => c.String());
            AlterColumn("dbo.Person", "FirstName", c => c.String());
        }
    }
}
