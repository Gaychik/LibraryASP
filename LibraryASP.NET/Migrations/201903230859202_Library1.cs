namespace LibraryASP.NET.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Library1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Person", "BirthDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Person", "BirthDate", c => c.Int(nullable: false));
        }
    }
}
