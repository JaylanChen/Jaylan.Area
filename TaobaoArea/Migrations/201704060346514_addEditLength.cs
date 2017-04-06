namespace TaobaoArea.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addEditLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TaobaoRegion", "Code", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AlterColumn("dbo.TaobaoRegion", "Name", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TaobaoRegion", "Name", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.TaobaoRegion", "Code", c => c.String(nullable: false, maxLength: 10, unicode: false));
        }
    }
}
