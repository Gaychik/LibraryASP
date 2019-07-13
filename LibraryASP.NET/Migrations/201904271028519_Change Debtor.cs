namespace LibraryASP.NET.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDebtor : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Person", "Book_BookID", "dbo.Book");
            DropIndex("dbo.Person", new[] { "Book_BookID" });
            AddColumn("dbo.History", "DeadLine", c => c.DateTime(nullable: false));
            DropColumn("dbo.Person", "Book_BookID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Person", "Book_BookID", c => c.Guid());
            DropColumn("dbo.History", "DeadLine");
            CreateIndex("dbo.Person", "Book_BookID");
            AddForeignKey("dbo.Person", "Book_BookID", "dbo.Book", "BookID");
        }
    }
}
