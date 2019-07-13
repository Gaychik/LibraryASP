namespace LibraryASP.NET.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangingEntityDebtorandHistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.History",
                c => new
                    {
                        HistoryID = c.Guid(nullable: false),
                        PersonID = c.Guid(nullable: false),
                        BookID = c.Guid(nullable: false),
                        Debtor_DebtorID = c.Guid(),
                    })
                .PrimaryKey(t => t.HistoryID)
                .ForeignKey("dbo.Book", t => t.BookID, cascadeDelete: true)
                .ForeignKey("dbo.Debtor", t => t.Debtor_DebtorID)
                .ForeignKey("dbo.Person", t => t.PersonID, cascadeDelete: true)
                .Index(t => t.PersonID)
                .Index(t => t.BookID)
                .Index(t => t.Debtor_DebtorID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.History", "PersonID", "dbo.Person");
            DropForeignKey("dbo.History", "Debtor_DebtorID", "dbo.Debtor");
            DropForeignKey("dbo.History", "BookID", "dbo.Book");
            DropIndex("dbo.History", new[] { "Debtor_DebtorID" });
            DropIndex("dbo.History", new[] { "BookID" });
            DropIndex("dbo.History", new[] { "PersonID" });
            DropTable("dbo.History");
        }
    }
}
