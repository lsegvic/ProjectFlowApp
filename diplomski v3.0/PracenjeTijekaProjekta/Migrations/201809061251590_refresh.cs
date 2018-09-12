namespace PracenjeTijekaProjekta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class refresh : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Projects", "ProjectID", "dbo.UsersProjects");
            DropIndex("dbo.Projects", new[] { "ProjectID" });
            DropPrimaryKey("dbo.Projects");
            AddColumn("dbo.Projects", "ProjectName", c => c.String(nullable: false));
            AlterColumn("dbo.Projects", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Projects", "Id");
            DropColumn("dbo.Projects", "ProjectID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Projects", "ProjectID", c => c.Int(nullable: false));
            DropPrimaryKey("dbo.Projects");
            AlterColumn("dbo.Projects", "Id", c => c.Int(nullable: false));
            DropColumn("dbo.Projects", "ProjectName");
            AddPrimaryKey("dbo.Projects", "Id");
            CreateIndex("dbo.Projects", "ProjectID");
            AddForeignKey("dbo.Projects", "ProjectID", "dbo.UsersProjects", "ProjectId", cascadeDelete: true);
        }
    }
}
