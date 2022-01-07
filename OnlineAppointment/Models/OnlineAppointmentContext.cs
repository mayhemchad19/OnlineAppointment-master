
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using Microsoft.OData.Edm;
using OnlineAppointment.Common;

namespace OnlineAppointment.Models
{
    public partial class OnlineAppointmentContext : DbContext
    {
        public OnlineAppointmentContext()
            : base("name=OnlineAppointmentContext1")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<User> Users { get; set; }



        public DbSet<Slot> Slots { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<AppointmentState> AppointmentStates { get; set; }


        public DbSet<PaymentType> PaymentTypes { get; set; }

        public DbSet<DiscountType> DiscountTypes { get; set; }

        public DbSet<Gender> Gender { get; set; }

        public DbSet<Billing> Billings { get; set; }

        //public DbSet<Transaction> Transactions { get; set; }


        public DbSet<Order> Orders { get; set; }


        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<Log> Logs { get; set; }

        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleDetail> SaleDetails { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<ScheduleDetail> ScheduleDetails { get; set; }

        public DbSet<AppointmentDetail> AppointmentDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }

        public System.Data.Entity.DbSet<OnlineAppointment.Models.Transactions> Transactions { get; set; }
    }

    public class Log
    {
        [Key]
        public int LogID { get; set; }

        [ForeignKey("User")]
        public int? UserID { get; set; }
        public User User { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0: MMM-dd-yyyy HH:mm:ss}")]
        public DateTime? LogDate { get; set; }
    }

    public class Billing
    {
        [Key]
        public int BillingID { get; set; }

        //Customer
        [ForeignKey("User")]
        public int? UserID { get; set; }

        public User User { get; set; }

        [ForeignKey("PaymentType")]
        public int? PaymentTypeID { get; set; }

        public PaymentType PaymentType { get; set; }
        [DataType(DataType.Date)]
        public DateTime? BillingDate { get; set; }
        public decimal FinalTotal { get; set; }
    }


    public class Sale
    {
        [Key]
        public int SaleID { get; set; }

        [ForeignKey("PaymentType")]
        public int? PaymentTypeID { get; set; }

        public PaymentType PaymentType { get; set; }

        //public string Employee { get; set; }


        //Customer
        [ForeignKey("User")]
        public int? UserID { get; set; }

        public User User { get; set; }


        public string OrderNumber { get; set; }


        [DataType(DataType.Date)]
        public DateTime? OrderDate { get; set; }

        [ForeignKey("DiscountType")]
        public int? DiscountTypeID { get; set; }

        public DiscountType DiscountType { get; set; }

        public decimal FinalTotal { get; set; }

        public decimal DiscountedTotal { get; set; }

        public bool? isPaid { get; set; }
        public bool? SaleStatus { get; set; }
        public IEnumerable<OrderDetail> ListOfOrderDetail { get; set; }
    }


    public class SaleDetail
    {
        [Key]
        public int OrderDetailID { get; set; }

        [ForeignKey("Sale")]
        public int? SaleID { get; set; }

        public Sale Sale { get; set; }

        [ForeignKey("Product")]
        public int? ProductID { get; set; }

        public Product Product { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public decimal Discount { get; set; }

        public decimal Total { get; set; }
    }


    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [ForeignKey("PaymentType")]
        public int? PaymentTypeID { get; set; }

        public PaymentType PaymentType { get; set; }

        //public string Employee { get; set; }


        //Customer
        [ForeignKey("User")]
        public int? UserID { get; set; }

        public User User { get; set; }


        public string OrderNumber { get; set; }


        [DataType(DataType.Date)]
        public DateTime? OrderDate { get; set; }

        [ForeignKey("DiscountType")]
        public int? DiscountTypeID { get; set; }

        public DiscountType DiscountType { get; set; }

        public decimal FinalTotal { get; set; }

        public decimal DiscountedTotal { get; set; }

        public bool? isPaid { get; set; }
        public bool? OrderStatus { get; set; }
        public IEnumerable<OrderDetail> ListOfOrderDetail { get; set; }
    }

    public class OrderDetail
    {
        [Key]
        public int OrderDetailID { get; set; }

        [ForeignKey("Order")]
        public int? OrderID { get; set; }

        public Order Order { get; set; }

        [ForeignKey("Product")]
        public int? ProductID { get; set; }

        public Product Product { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public decimal Discount { get; set; }

        public decimal Total { get; set; }
    }

    public class Gender
    {
        [Key]
        public int GenderID { get; set; }
        public string GenderType { get; set; }
    }

    public class PaymentType
    {
        [Key]
        public int PaymentTypeID { get; set; }
        public string PaymentTypeName { get; set; }
        public bool? PaymentTypeStatus { get; set; }
    }

    public class DiscountType
    {
        [Key]
        public int DiscountTypeID { get; set; }

        [DisplayName("Discount")]
        public string DiscountTypeName { get; set; }

        public double DiscountAmount { get; set; }
        public bool? DiscountTypeStatus { get; set; }


    }


    // //last
    public class Appointment
    {
        public int AppointmentId { get; set; }

        [ForeignKey("User")]
        public int? UserID { get; set; }
        public User User { get; set; }

        [ForeignKey("Product")]

        public int? ProductID { get; set; }
        [Column("Service")]
        public Product Product { get; set; }

        //[AppointmentDate(ErrorMessage = ("Appointment Date should be set today and onward."))]
        [DisplayFormatAttribute(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateTime? AppointmentDate { get; set; }

        [ForeignKey("Slot")]
        public int? SlotID { get; set; }
        public Slot Slot { get; set; }

        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public string Reason { get; set; }

        public string Status { get; set; }

        public decimal Total { get; set; }
        public decimal Balance { get; set; }
        public decimal Discount { get; set; }

        [ForeignKey("AppointmentState")]
        public int? AppointmentStateID { get; set; }
        public AppointmentState AppointmentState { get; set; }
    }

    public class AppointmentDetail
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Appointment")]
        public int? AppointmentId { get; set; }
        public Appointment Appointment { get; set; }

        [ForeignKey("Product")]
        [Display(Name = "Product/Service")]
        public int? ProductID { get; set; }
        public Product Product { get; set; }
        [Required]
      
        [Range(1, 1000)]
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        [Range(0, 100)]
        public decimal? Discount { get; set; }
        public decimal? Total { get; set; }
    }

    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Appointment")]
        public int? AppointmentId { get; set; }
        public Appointment Appointment { get; set; }

        public string InvoiceNo { get; set; }
        public decimal? Amount { get; set; }

    }


    public class Schedule
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int? UserID { get; set; }
        public User User { get; set; }

        [DisplayFormatAttribute(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public DateTime? ScheduleDate { get; set; }

        public string Status { get; set; }
        public string Remarks { get; set; }

    }

    public class ScheduleDetail
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Schedule")]
        public int? ScheduleId { get; set; }
        public Schedule Schedule { get; set; }

        [ForeignKey("Product")]
        public int? ProductID { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? Total { get; set; }
    }




    public class Slot
    {
        [Key]
        [DisplayName("Time")]
        public int SlotsID { get; set; }

        [Display(Name = "Description")]
        public string Time { get; set; }

        public int AvailableSlot { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime StartTime { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime EndTime { get; set; }


        public string Schedule
        {
            get { return StartTime.ToShortTimeString() + " - " + EndTime.ToShortTimeString(); }
        }

    }

    public class AppointmentState
    {
        [Key]
        public int AppointmentStateID { get; set; }
        public string AppointmentStatus { get; set; }
    }


    public class ProductType
    {
        [Key]
        public int ProductTypeID { get; set; }
        [DisplayName("Type")]
        public string ProductTypeName { get; set; }
        public bool? ProductTypeStatus { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }

    public class Product
    {
        [Key]
        public int ProductID { get; set; }
        [Required]
        [DisplayName("Product/Service")]
        public string ProductName { get; set; }
        [Required]
        [DisplayName("Price")]
        [Range(0, 100000)]
        public decimal ProductPrice { get; set; }
        public int ProductTypeID { get; set; }
        public virtual ProductType ProductType { get; set; }
        [Required]
        [DisplayName("Description")]
        public string ProductDescription { get; set; }
        public bool? ProductStatus { get; set; }

    }

    public class UserRole
    {
        [Key]
        public int RoleID { get; set; }
        public string RoleName { get; set; }

        public int RoleLevel { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }

    public class User
    {
        [Key]
        public int UserID { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 2)]
        public string Password { get; set; }
        [Required]
        [DisplayName("First Name")]
        [StringLength(20, MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z\s\u00f1\u00d1]*$", ErrorMessage = "Special characters and numbers are not allowed")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        [RegularExpression(@"^[a-zA-Z\s\u00f1\u00d1]*$", ErrorMessage = "Special characters and numbers are not allowed")]
        [StringLength(20, MinimumLength = 1)]
        public string LastName { get; set; }

        public string Gender { get; set; }

        [DisplayName("Birthdate")]
        [CurrentDate(ErrorMessage = ("Birthdate is invalid"))]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DisplayName("Mobile No.")]

        [RegularExpression("(^0[1-9][0-9]*)", ErrorMessage = "Please a correct mobile number format. (e.g. 09XX XXXX XXX)")]
        [Range(9000000000, 9999999999, ErrorMessage = "Please enter a correct mobile number format. (e.g. 09XX XXXX XXX)")]
        public string MobileNumber { get; set; }

        public int RoleID { get; set; }
        public virtual UserRole UserRole { get; set; }

        [DisplayName("User Status")]
        public bool? UserStatus { get; set; }

        public bool? isVerified { get; set; }
        public DateTime? RegDate { get; set; }

        public string Name
        {
            get { return LastName + ", " + FirstName; }
        }

    }
}
