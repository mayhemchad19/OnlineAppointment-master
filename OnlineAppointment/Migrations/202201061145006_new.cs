namespace OnlineAppointment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _new : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppointmentDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppointmentId = c.Int(),
                        ProductID = c.Int(),
                        Quantity = c.Int(),
                        Price = c.Decimal(precision: 18, scale: 2),
                        Discount = c.Decimal(precision: 18, scale: 2),
                        Total = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Appointments", t => t.AppointmentId)
                .ForeignKey("dbo.Products", t => t.ProductID)
                .Index(t => t.AppointmentId)
                .Index(t => t.ProductID);
            
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        AppointmentId = c.Int(nullable: false, identity: true),
                        UserID = c.Int(),
                        ProductID = c.Int(),
                        AppointmentDate = c.DateTime(),
                        SlotID = c.Int(),
                        Reason = c.String(),
                        Status = c.String(),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AppointmentStateID = c.Int(),
                    })
                .PrimaryKey(t => t.AppointmentId)
                .ForeignKey("dbo.AppointmentStates", t => t.AppointmentStateID)
                .ForeignKey("dbo.Products", t => t.ProductID)
                .ForeignKey("dbo.Slots", t => t.SlotID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.UserID)
                .Index(t => t.ProductID)
                .Index(t => t.SlotID)
                .Index(t => t.AppointmentStateID);
            
            CreateTable(
                "dbo.AppointmentStates",
                c => new
                    {
                        AppointmentStateID = c.Int(nullable: false, identity: true),
                        AppointmentStatus = c.String(),
                    })
                .PrimaryKey(t => t.AppointmentStateID);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductID = c.Int(nullable: false, identity: true),
                        ProductName = c.String(nullable: false),
                        ProductPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProductTypeID = c.Int(nullable: false),
                        ProductDescription = c.String(nullable: false),
                        ProductStatus = c.Boolean(),
                    })
                .PrimaryKey(t => t.ProductID)
                .ForeignKey("dbo.ProductTypes", t => t.ProductTypeID, cascadeDelete: true)
                .Index(t => t.ProductTypeID);
            
            CreateTable(
                "dbo.ProductTypes",
                c => new
                    {
                        ProductTypeID = c.Int(nullable: false, identity: true),
                        ProductTypeName = c.String(),
                        ProductTypeStatus = c.Boolean(),
                    })
                .PrimaryKey(t => t.ProductTypeID);
            
            CreateTable(
                "dbo.Slots",
                c => new
                    {
                        SlotsID = c.Int(nullable: false, identity: true),
                        Time = c.String(),
                        AvailableSlot = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SlotsID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 20),
                        Password = c.String(nullable: false, maxLength: 20),
                        FirstName = c.String(nullable: false, maxLength: 20),
                        LastName = c.String(maxLength: 20),
                        Gender = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                        Email = c.String(nullable: false),
                        MobileNumber = c.String(),
                        RoleID = c.Int(nullable: false),
                        UserStatus = c.Boolean(),
                        isVerified = c.Boolean(),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.UserRoles", t => t.RoleID, cascadeDelete: true)
                .Index(t => t.RoleID);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        RoleID = c.Int(nullable: false, identity: true),
                        RoleName = c.String(),
                        RoleLevel = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RoleID);
            
            CreateTable(
                "dbo.Billings",
                c => new
                    {
                        BillingID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(),
                        PaymentTypeID = c.Int(),
                        BillingDate = c.DateTime(),
                        FinalTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.BillingID)
                .ForeignKey("dbo.PaymentTypes", t => t.PaymentTypeID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.UserID)
                .Index(t => t.PaymentTypeID);
            
            CreateTable(
                "dbo.PaymentTypes",
                c => new
                    {
                        PaymentTypeID = c.Int(nullable: false, identity: true),
                        PaymentTypeName = c.String(),
                        PaymentTypeStatus = c.Boolean(),
                    })
                .PrimaryKey(t => t.PaymentTypeID);
            
            CreateTable(
                "dbo.DiscountTypes",
                c => new
                    {
                        DiscountTypeID = c.Int(nullable: false, identity: true),
                        DiscountTypeName = c.String(),
                        DiscountAmount = c.Double(nullable: false),
                        DiscountTypeStatus = c.Boolean(),
                    })
                .PrimaryKey(t => t.DiscountTypeID);
            
            CreateTable(
                "dbo.Genders",
                c => new
                    {
                        GenderID = c.Int(nullable: false, identity: true),
                        GenderType = c.String(),
                    })
                .PrimaryKey(t => t.GenderID);
            
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        LogID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(),
                        LogDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.LogID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        OrderDetailID = c.Int(nullable: false, identity: true),
                        OrderID = c.Int(),
                        ProductID = c.Int(),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.OrderDetailID)
                .ForeignKey("dbo.Orders", t => t.OrderID)
                .ForeignKey("dbo.Products", t => t.ProductID)
                .Index(t => t.OrderID)
                .Index(t => t.ProductID);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        PaymentTypeID = c.Int(),
                        UserID = c.Int(),
                        OrderNumber = c.String(),
                        OrderDate = c.DateTime(),
                        DiscountTypeID = c.Int(),
                        FinalTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountedTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        isPaid = c.Boolean(),
                        OrderStatus = c.Boolean(),
                    })
                .PrimaryKey(t => t.OrderID)
                .ForeignKey("dbo.DiscountTypes", t => t.DiscountTypeID)
                .ForeignKey("dbo.PaymentTypes", t => t.PaymentTypeID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.PaymentTypeID)
                .Index(t => t.UserID)
                .Index(t => t.DiscountTypeID);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppointmentId = c.Int(),
                        InvoiceNo = c.String(),
                        Amount = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Appointments", t => t.AppointmentId)
                .Index(t => t.AppointmentId);
            
            CreateTable(
                "dbo.SaleDetails",
                c => new
                    {
                        OrderDetailID = c.Int(nullable: false, identity: true),
                        SaleID = c.Int(),
                        ProductID = c.Int(),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.OrderDetailID)
                .ForeignKey("dbo.Products", t => t.ProductID)
                .ForeignKey("dbo.Sales", t => t.SaleID)
                .Index(t => t.SaleID)
                .Index(t => t.ProductID);
            
            CreateTable(
                "dbo.Sales",
                c => new
                    {
                        SaleID = c.Int(nullable: false, identity: true),
                        PaymentTypeID = c.Int(),
                        UserID = c.Int(),
                        OrderNumber = c.String(),
                        OrderDate = c.DateTime(),
                        DiscountTypeID = c.Int(),
                        FinalTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountedTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        isPaid = c.Boolean(),
                        SaleStatus = c.Boolean(),
                    })
                .PrimaryKey(t => t.SaleID)
                .ForeignKey("dbo.DiscountTypes", t => t.DiscountTypeID)
                .ForeignKey("dbo.PaymentTypes", t => t.PaymentTypeID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.PaymentTypeID)
                .Index(t => t.UserID)
                .Index(t => t.DiscountTypeID);
            
            CreateTable(
                "dbo.ScheduleDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ScheduleId = c.Int(),
                        ProductID = c.Int(),
                        Quantity = c.Int(nullable: false),
                        Price = c.Decimal(precision: 18, scale: 2),
                        Total = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductID)
                .ForeignKey("dbo.Schedules", t => t.ScheduleId)
                .Index(t => t.ScheduleId)
                .Index(t => t.ProductID);
            
            CreateTable(
                "dbo.Schedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserID = c.Int(),
                        ScheduleDate = c.DateTime(),
                        Status = c.String(),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerName = c.String(),
                        SlotTime = c.String(),
                        ScheduledDate = c.DateTime(),
                        Discount = c.Decimal(precision: 18, scale: 2),
                        Balance = c.Decimal(precision: 18, scale: 2),
                        Total = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ScheduleDetails", "ScheduleId", "dbo.Schedules");
            DropForeignKey("dbo.Schedules", "UserID", "dbo.Users");
            DropForeignKey("dbo.ScheduleDetails", "ProductID", "dbo.Products");
            DropForeignKey("dbo.SaleDetails", "SaleID", "dbo.Sales");
            DropForeignKey("dbo.Sales", "UserID", "dbo.Users");
            DropForeignKey("dbo.Sales", "PaymentTypeID", "dbo.PaymentTypes");
            DropForeignKey("dbo.Sales", "DiscountTypeID", "dbo.DiscountTypes");
            DropForeignKey("dbo.SaleDetails", "ProductID", "dbo.Products");
            DropForeignKey("dbo.Payments", "AppointmentId", "dbo.Appointments");
            DropForeignKey("dbo.OrderDetails", "ProductID", "dbo.Products");
            DropForeignKey("dbo.OrderDetails", "OrderID", "dbo.Orders");
            DropForeignKey("dbo.Orders", "UserID", "dbo.Users");
            DropForeignKey("dbo.Orders", "PaymentTypeID", "dbo.PaymentTypes");
            DropForeignKey("dbo.Orders", "DiscountTypeID", "dbo.DiscountTypes");
            DropForeignKey("dbo.Logs", "UserID", "dbo.Users");
            DropForeignKey("dbo.Billings", "UserID", "dbo.Users");
            DropForeignKey("dbo.Billings", "PaymentTypeID", "dbo.PaymentTypes");
            DropForeignKey("dbo.AppointmentDetails", "ProductID", "dbo.Products");
            DropForeignKey("dbo.AppointmentDetails", "AppointmentId", "dbo.Appointments");
            DropForeignKey("dbo.Appointments", "UserID", "dbo.Users");
            DropForeignKey("dbo.Users", "RoleID", "dbo.UserRoles");
            DropForeignKey("dbo.Appointments", "SlotID", "dbo.Slots");
            DropForeignKey("dbo.Appointments", "ProductID", "dbo.Products");
            DropForeignKey("dbo.Products", "ProductTypeID", "dbo.ProductTypes");
            DropForeignKey("dbo.Appointments", "AppointmentStateID", "dbo.AppointmentStates");
            DropIndex("dbo.Schedules", new[] { "UserID" });
            DropIndex("dbo.ScheduleDetails", new[] { "ProductID" });
            DropIndex("dbo.ScheduleDetails", new[] { "ScheduleId" });
            DropIndex("dbo.Sales", new[] { "DiscountTypeID" });
            DropIndex("dbo.Sales", new[] { "UserID" });
            DropIndex("dbo.Sales", new[] { "PaymentTypeID" });
            DropIndex("dbo.SaleDetails", new[] { "ProductID" });
            DropIndex("dbo.SaleDetails", new[] { "SaleID" });
            DropIndex("dbo.Payments", new[] { "AppointmentId" });
            DropIndex("dbo.Orders", new[] { "DiscountTypeID" });
            DropIndex("dbo.Orders", new[] { "UserID" });
            DropIndex("dbo.Orders", new[] { "PaymentTypeID" });
            DropIndex("dbo.OrderDetails", new[] { "ProductID" });
            DropIndex("dbo.OrderDetails", new[] { "OrderID" });
            DropIndex("dbo.Logs", new[] { "UserID" });
            DropIndex("dbo.Billings", new[] { "PaymentTypeID" });
            DropIndex("dbo.Billings", new[] { "UserID" });
            DropIndex("dbo.Users", new[] { "RoleID" });
            DropIndex("dbo.Products", new[] { "ProductTypeID" });
            DropIndex("dbo.Appointments", new[] { "AppointmentStateID" });
            DropIndex("dbo.Appointments", new[] { "SlotID" });
            DropIndex("dbo.Appointments", new[] { "ProductID" });
            DropIndex("dbo.Appointments", new[] { "UserID" });
            DropIndex("dbo.AppointmentDetails", new[] { "ProductID" });
            DropIndex("dbo.AppointmentDetails", new[] { "AppointmentId" });
            DropTable("dbo.Transactions");
            DropTable("dbo.Schedules");
            DropTable("dbo.ScheduleDetails");
            DropTable("dbo.Sales");
            DropTable("dbo.SaleDetails");
            DropTable("dbo.Payments");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.Logs");
            DropTable("dbo.Genders");
            DropTable("dbo.DiscountTypes");
            DropTable("dbo.PaymentTypes");
            DropTable("dbo.Billings");
            DropTable("dbo.UserRoles");
            DropTable("dbo.Users");
            DropTable("dbo.Slots");
            DropTable("dbo.ProductTypes");
            DropTable("dbo.Products");
            DropTable("dbo.AppointmentStates");
            DropTable("dbo.Appointments");
            DropTable("dbo.AppointmentDetails");
        }
    }
}
