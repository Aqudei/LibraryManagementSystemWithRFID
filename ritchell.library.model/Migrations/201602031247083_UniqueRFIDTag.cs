namespace ritchell.library.model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UniqueRFIDTag : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BookCopies", "BookTagShort", c => c.String(maxLength: 255, storeType: "varchar"));
            AlterColumn("dbo.BookCopies", "BookTagLong", c => c.String(maxLength: 255, storeType: "varchar"));
            CreateIndex("dbo.BookCopies", "BookTagShort", unique: true);
            CreateIndex("dbo.BookCopies", "BookTagLong", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.BookCopies", new[] { "BookTagLong" });
            DropIndex("dbo.BookCopies", new[] { "BookTagShort" });
            AlterColumn("dbo.BookCopies", "BookTagLong", c => c.String(unicode: false));
            AlterColumn("dbo.BookCopies", "BookTagShort", c => c.String(unicode: false));
        }
    }
}
