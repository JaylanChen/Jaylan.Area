namespace NBSArea.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NBSRegion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentCode = c.String(nullable: false, maxLength: 10, unicode: false),
                        Code = c.String(nullable: false, maxLength: 20, unicode: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        ZipCode = c.String(maxLength: 10, unicode: false),
                        Level = c.Int(nullable: false),
                        IsDel = c.Boolean(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.NBSRegion");
        }
    }
}
