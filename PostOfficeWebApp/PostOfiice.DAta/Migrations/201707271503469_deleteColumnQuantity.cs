namespace PostOfiice.DAta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteColumnQuantity : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TransactionDetails", "Quantity");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TransactionDetails", "Quantity", c => c.Int());
        }
    }
}
