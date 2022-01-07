using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineAppointment.Models
{
    public class ViewModels
    {
    }

    public class SaleViewMOdel
    {
        public int SaleID { get; set; }
        public string  Customer { get; set; }
        public string OrderNumber { get; set; }
        public decimal? FinalTotal { get; set; }
        public decimal? Discount { get; set; }

        public string DiscountType { get; set; }

        public decimal? DiscountedPrice { get; set; } //try mo daw
    }


    public class SaleSummaryViewMOdel
    {
        public string SaleNumber { get; set; }
        public DateTime? Date { get; set; }
        //public string Date { get; set; }
        public string Customer { get; set; }
        public string Product { get; set; }
      
        public decimal? UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal? Discount { get; set; }

        public decimal? Total { get; set; }

        public decimal? totaltotal { get; set; }

    }

    public class Transactions
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string SlotTime { get; set; }

        [DisplayFormatAttribute(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? ScheduledDate { get; set; }

        public decimal? Discount { get; set; }
        public decimal? Balance { get; set; }
        public decimal? Total { get; set; }
    }





}