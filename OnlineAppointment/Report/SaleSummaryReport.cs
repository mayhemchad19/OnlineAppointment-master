using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using OnlineAppointment.Models;

namespace OnlineAppointment.Report
{
    public class SaleSummaryReport
    {
        public decimal? x;

        #region Declaration
        int _totalColumn = 9;

        Document _document;
        Font _fontStyle;
        PdfPTable _pdfPtable = new PdfPTable(9);
        PdfPCell _pdfPcell;
        MemoryStream _memoryStream = new MemoryStream();
        List<SaleSummaryViewMOdel> _sales = new List<SaleSummaryViewMOdel>();
        #endregion




        public byte[] PrepareSalesReport(List<SaleSummaryViewMOdel> sale)
        {//
            //PdfPTable _pdfPtable = new PdfPTable(6);
            _sales = sale;
            #region
            _document = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _document.SetPageSize(PageSize.A4_LANDSCAPE);
            _document.SetMargins(20f, 20f, 20f, 20f);
            _pdfPtable.WidthPercentage = 100;
            _pdfPtable.HorizontalAlignment = Element.ALIGN_LEFT;
            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_document, _memoryStream);
            _document.Open();
            //_pdfPtable.SetWidths(new float[] { 20f, 150f, 100f });
            _pdfPtable.SetWidths(new float[] { 10f,50f, 60f, 40f, 50f, 30f, 30f, 40f, 40f });
            #endregion

            this.SalesReportHeader();
            this.SalesReportBody();
            _pdfPtable.HeaderRows = 2;
            _document.Add(_pdfPtable);
            _document.Close();
            return _memoryStream.ToArray();


        }
        //string duration = Session["Duration"].toString;
        private void SalesReportHeader()
        {
            _fontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
            _pdfPcell = new PdfPCell(new Phrase("Sales Report", _fontStyle));
            _pdfPcell.Colspan = _totalColumn;
            _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPcell.Border = 0;
            _pdfPcell.BackgroundColor = BaseColor.WHITE;
            _pdfPcell.ExtraParagraphSpace = 1;
            _pdfPtable.AddCell(_pdfPcell);
            _pdfPtable.CompleteRow();

            //_fontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            //_pdfPcell = new PdfPCell(new Phrase(Session["Duration"], _fontStyle));
            //_pdfPcell.Colspan = _totalColumn;
            //_pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
            //_pdfPcell.Border = 0;
            //_pdfPcell.BackgroundColor = BaseColor.WHITE;
            //_pdfPcell.ExtraParagraphSpace = 0;
            //_pdfPtable.AddCell(_pdfPcell);
            //_pdfPtable.CompleteRow();

        }

        private void SalesReportBody()
        {
            #region Table header
            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _pdfPcell = new PdfPCell(new Phrase("", _fontStyle));
            _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPcell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPcell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPtable.AddCell(_pdfPcell);

            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _pdfPcell = new PdfPCell(new Phrase("Sale Number", _fontStyle));
            _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPcell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPcell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPtable.AddCell(_pdfPcell);

            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _pdfPcell = new PdfPCell(new Phrase("Date", _fontStyle));
            _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPcell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPcell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPtable.AddCell(_pdfPcell);

            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _pdfPcell = new PdfPCell(new Phrase("Customer ID", _fontStyle));
            _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPcell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPcell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPtable.AddCell(_pdfPcell);

            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _pdfPcell = new PdfPCell(new Phrase("Product/Service", _fontStyle));
            _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPcell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPcell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPtable.AddCell(_pdfPcell);

            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _pdfPcell = new PdfPCell(new Phrase("Unit Price", _fontStyle));
            _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPcell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPcell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPtable.AddCell(_pdfPcell);

            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _pdfPcell = new PdfPCell(new Phrase("Quantity", _fontStyle));
            _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPcell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPcell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPtable.AddCell(_pdfPcell);

            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _pdfPcell = new PdfPCell(new Phrase("Item Discount", _fontStyle));
            _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPcell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPcell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPtable.AddCell(_pdfPcell);

            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _pdfPcell = new PdfPCell(new Phrase("Total", _fontStyle));
            _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPcell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPcell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPtable.AddCell(_pdfPcell);
            _pdfPtable.CompleteRow();
            #endregion

            #region Table Body
            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            int i = 1;
            
            foreach (SaleSummaryViewMOdel sale in _sales)
            {
                _pdfPcell = new PdfPCell(new Phrase(i++.ToString(), _fontStyle));
                _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPcell.BackgroundColor = BaseColor.WHITE;
                this.x += sale.Total;
                _pdfPtable.AddCell(_pdfPcell);

                _pdfPcell = new PdfPCell(new Phrase(sale.SaleNumber, _fontStyle));
                _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPcell.BackgroundColor = BaseColor.WHITE;
                _pdfPtable.AddCell(_pdfPcell);

                _pdfPcell = new PdfPCell(new Phrase(sale.Date.ToString(), _fontStyle));
                //_pdfPcell = new PdfPCell(new Phrase(sale.User.FirstName, _fontStyle));
                _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPcell.BackgroundColor = BaseColor.WHITE;
                _pdfPtable.AddCell(_pdfPcell);

                //_pdfPcell = new PdfPCell(new Phrase(saleDetail.ProductID.ToString(), _fontStyle)) ;
                _pdfPcell = new PdfPCell(new Phrase(sale.Customer.ToString(), _fontStyle));
                _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPcell.BackgroundColor = BaseColor.WHITE;
                _pdfPtable.AddCell(_pdfPcell);

                _pdfPcell = new PdfPCell(new Phrase(sale.Product, _fontStyle));
                _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPcell.BackgroundColor = BaseColor.WHITE;
                _pdfPtable.AddCell(_pdfPcell);

                _pdfPcell = new PdfPCell(new Phrase(sale.UnitPrice.ToString(), _fontStyle));
                _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPcell.BackgroundColor = BaseColor.WHITE;
                _pdfPtable.AddCell(_pdfPcell);

                //_pdfPcell = new PdfPCell(new Phrase(saleDetail.ProductID.ToString(), _fontStyle)) ;
                _pdfPcell = new PdfPCell(new Phrase(sale.Quantity.ToString(), _fontStyle));
                _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPcell.BackgroundColor = BaseColor.WHITE;
                _pdfPtable.AddCell(_pdfPcell);

                _pdfPcell = new PdfPCell(new Phrase(sale.Discount.ToString(), _fontStyle));
                _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPcell.BackgroundColor = BaseColor.WHITE;
                _pdfPtable.AddCell(_pdfPcell);

                _pdfPcell = new PdfPCell(new Phrase(sale.Total.ToString(), _fontStyle));
                _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPcell.BackgroundColor = BaseColor.WHITE;
                _pdfPtable.AddCell(_pdfPcell);
                _pdfPtable.CompleteRow();
            }
            #endregion
            var tot = (from x in _sales select x.totaltotal).Last();
          
            _fontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
            _pdfPcell = new PdfPCell(new Phrase(tot.ToString(), _fontStyle));
            _pdfPcell.Colspan = _totalColumn;
            _pdfPcell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _pdfPcell.Border = 0;
            _pdfPcell.BackgroundColor = BaseColor.WHITE;
            _pdfPcell.ExtraParagraphSpace = 0;
            _pdfPtable.AddCell(_pdfPcell);
            _pdfPtable.CompleteRow();

            _fontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
            _pdfPcell = new PdfPCell(new Phrase("Date: " +DateTime.Now.ToString(), _fontStyle));
            _pdfPcell.Colspan = _totalColumn;
            _pdfPcell.HorizontalAlignment = Element.ALIGN_RIGHT;
            _pdfPcell.Border = 0;
            _pdfPcell.BackgroundColor = BaseColor.WHITE;
            _pdfPcell.ExtraParagraphSpace = 0;
            _pdfPtable.AddCell(_pdfPcell);
            _pdfPtable.CompleteRow();

        }
    }
    }