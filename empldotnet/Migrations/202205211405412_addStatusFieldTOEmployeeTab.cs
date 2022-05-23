namespace empldotnet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addStatusFieldTOEmployeeTab : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "Status");
        }
    }
}
