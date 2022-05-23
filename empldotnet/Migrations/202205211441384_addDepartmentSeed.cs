namespace empldotnet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDepartmentSeed : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Departments (dep) VALUES ('FirstName') ;");
            Sql("INSERT INTO Departments (dep) VALUES ('LastName') ;");
            Sql("INSERT INTO Departments (dep) VALUES ('Date') ;");
            Sql("INSERT INTO Departments (dep) VALUES ('Department') ;");
        }
        
        public override void Down()
        {
        }
    }
}
