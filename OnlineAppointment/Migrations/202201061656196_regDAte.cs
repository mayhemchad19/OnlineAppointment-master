namespace OnlineAppointment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class regDAte : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "RegDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "RegDate");
        }
    }
}
