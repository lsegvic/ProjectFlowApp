namespace PracenjeTijekaProjekta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.UsersProjects", "Date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UsersProjects", "Date", c => c.DateTime(nullable: false));
        }
    }
}
