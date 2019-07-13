namespace LibraryASP.NET.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Book",
                c => new
                    {
                        BookID = c.Guid(nullable: false),
                        Name = c.String(),
                        Author = c.String(),
                        DateEdition = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.BookID);
            
            CreateTable(
                "dbo.Person",
                c => new
                    {
                        PersonID = c.Guid(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        MidName = c.String(),
                        Age = c.Int(nullable: false),
                        Address = c.String(),
                        TelephoneNumber = c.String(),
                    })
                .PrimaryKey(t => t.PersonID);
            
            CreateTable(
                "dbo.Debtor",
                c => new
                    {
                        DebtorID = c.Guid(nullable: false),
                        PersonID = c.Guid(nullable: false),
                        DateIssue = c.DateTime(nullable: false),
                        DateReceipt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DebtorID)
                .ForeignKey("dbo.Person", t => t.PersonID, cascadeDelete: true)
                .Index(t => t.PersonID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Debtor", "PersonID", "dbo.Person");
            DropIndex("dbo.Debtor", new[] { "PersonID" });
            DropTable("dbo.Debtor");
            DropTable("dbo.Person");
            DropTable("dbo.Book");
        }
    }
}
