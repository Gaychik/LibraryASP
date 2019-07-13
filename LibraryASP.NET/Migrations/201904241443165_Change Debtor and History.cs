namespace LibraryASP.NET.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDebtorandHistory : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Debtor", "History_HistoryID", "dbo.History");
            DropForeignKey("dbo.Debtor", "PersonID", "dbo.Person");
            DropIndex("dbo.Debtor", new[] { "PersonID" });
            DropIndex("dbo.Debtor", new[] { "History_HistoryID" });
            DropTable("dbo.Debtor");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Debtor",
                c => new
                    {
                        DebtorID = c.Guid(nullable: false),
                        PersonID = c.Guid(nullable: false),
                        History_HistoryID = c.Guid(),
                    })
                .PrimaryKey(t => t.DebtorID);
            
            CreateIndex("dbo.Debtor", "History_HistoryID");
            CreateIndex("dbo.Debtor", "PersonID");
            AddForeignKey("dbo.Debtor", "PersonID", "dbo.Person", "PersonID", cascadeDelete: true);
            AddForeignKey("dbo.Debtor", "History_HistoryID", "dbo.History", "HistoryID");
        }
    }
}
