namespace MiArea.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MiCity",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MiId = c.Int(nullable: false),
                        ParentId = c.Int(nullable: false),
                        Level = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        ZipCode = c.String(maxLength: 10, unicode: false),
                        Status = c.Int(nullable: false),
                        IsDel = c.Boolean(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MiCity");
        }
    }
}
