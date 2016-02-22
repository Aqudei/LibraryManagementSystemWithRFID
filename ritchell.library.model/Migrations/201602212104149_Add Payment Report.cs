namespace ritchell.library.model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddPaymentReport : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransactionInfoes", "DateOfPayment", c => c.DateTime(nullable: false, precision: 0, defaultValue: DateTime.Now));
        }

        public override void Down()
        {
            DropColumn("dbo.TransactionInfoes", "DateOfPayment");
        }
    }
}
