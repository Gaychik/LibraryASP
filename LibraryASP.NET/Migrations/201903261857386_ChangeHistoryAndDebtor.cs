namespace LibraryASP.NET.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeHistoryAndDebtor : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.History", "Debtor_DebtorID", "dbo.Debtor");
            DropIndex("dbo.History", new[] { "Debtor_DebtorID" });
            AddColumn("dbo.Debtor", "History_HistoryID", c => c.Guid());
            AddColumn("dbo.Person", "Book_BookID", c => c.Guid());
            AddColumn("dbo.History", "DateIssue", c => c.DateTime(nullable: false));
            AddColumn("dbo.History", "DateReceipt", c => c.DateTime(nullable: false));
            CreateIndex("dbo.Debtor", "History_HistoryID");
            CreateIndex("dbo.Person", "Book_BookID");
            AddForeignKey("dbo.Person", "Book_BookID", "dbo.Book", "BookID");
            AddForeignKey("dbo.Debtor", "History_HistoryID", "dbo.History", "HistoryID");
            DropColumn("dbo.Debtor", "DateIssue");
            DropColumn("dbo.Debtor", "DateReceipt");
            DropColumn("dbo.History", "Debtor_DebtorID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.History", "Debtor_DebtorID", c => c.Guid());
            AddColumn("dbo.Debtor", "DateReceipt", c => c.DateTime(nullable: false));
            AddColumn("dbo.Debtor", "DateIssue", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.Debtor", "History_HistoryID", "dbo.History");
            DropForeignKey("dbo.Person", "Book_BookID", "dbo.Book");
            DropIndex("dbo.Person", new[] { "Book_BookID" });
            DropIndex("dbo.Debtor", new[] { "History_HistoryID" });
            DropColumn("dbo.History", "DateReceipt");
            DropColumn("dbo.History", "DateIssue");
            DropColumn("dbo.Person", "Book_BookID");
            DropColumn("dbo.Debtor", "History_HistoryID");
            CreateIndex("dbo.History", "Debtor_DebtorID");
            AddForeignKey("dbo.History", "Debtor_DebtorID", "dbo.Debtor", "DebtorID");
        }
    }
}
