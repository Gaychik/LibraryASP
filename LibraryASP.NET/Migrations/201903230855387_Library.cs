namespace LibraryASP.NET.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Library : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Person", "BirthDate", c => c.Int(nullable: false));
            DropColumn("dbo.Person", "Age");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Person", "Age", c => c.Int(nullable: false));
            DropColumn("dbo.Person", "BirthDate");
        }
    }
}
