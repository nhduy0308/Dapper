namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewTableSaleAndSaleItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SaleItems",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        SaleId = c.Guid(nullable: false),
                        Name = c.String(),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.SaleId })
                .ForeignKey("dbo.Sale", t => t.Id, cascadeDelete: true)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Sale",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        CreatedDate = c.DateTime(),
                        CreatedBy = c.String(maxLength: 256),
                        UpdatedDate = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 256),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SaleItems", "Id", "dbo.Sale");
            DropIndex("dbo.SaleItems", new[] { "Id" });
            DropTable("dbo.Sale");
            DropTable("dbo.SaleItems");
        }
    }
}
