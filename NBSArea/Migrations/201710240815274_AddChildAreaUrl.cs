namespace NBSArea.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddChildAreaUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NBSArea", "IsGetChild", c => c.Boolean(nullable: false));
            AddColumn("dbo.NBSArea", "ChildNodeUrl", c => c.String(maxLength: 512, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.NBSArea", "ChildNodeUrl");
            DropColumn("dbo.NBSArea", "IsGetChild");
        }
    }
}
