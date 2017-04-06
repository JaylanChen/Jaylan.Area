namespace TaobaoArea.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addColumnStstus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaobaoRegion", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaobaoRegion", "Status");
        }
    }
}
