namespace ritchell.library.model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentDateNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TransactionInfoes", "DateOfPayment", c => c.DateTime(precision: 0));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TransactionInfoes", "DateOfPayment", c => c.DateTime(nullable: false, precision: 0));
        }
    }
}
