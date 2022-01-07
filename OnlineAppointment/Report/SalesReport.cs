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
    public class SalesReport
    {
        #region Declaration
        int _totalColumn = 3;
        int _totalColumn2 = 6;
        Document _document;
        Font _fontStyle;
        PdfPTable _pdfPtable = new PdfPTable(6);
        PdfPCell _pdfPcell;
        MemoryStream _memoryStream = new MemoryStream();
        List<SaleDetail> _saleDetails = new List<SaleDetail>();
        List<SaleViewMOdel> _sales = new List<SaleViewMOdel>();
        #endregion

        public byte[] PrepareReport(List<SaleDetail> saleDetails)
        {
            _saleDetails = saleDetails;
            #region
            _document = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _document.SetPageSize(PageSize.A4_LANDSCAPE);
            _document.SetMargins(20f, 20f, 20f, 20f);
            _pdfPtable.WidthPercentage = 100;
            _pdfPtable.HorizontalAlignment = Element.ALIGN_LEFT;
            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_document, _memoryStream);
            _document.Open();
            _pdfPtable.SetWidths(new float[] { 20f, 150f, 100f });
            #endregion

            this.ReportHeader();
            this.ReportBody();
            _pdfPtable.HeaderRows = 2;
            _document.Add(_pdfPtable);
            _document.Close();
            return _memoryStream.ToArray();


        }
        private void ReportHeader()
        {
            _fontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
            _pdfPcell = new PdfPCell(new Phrase("Sales Report", _fontStyle));
            _pdfPcell.Colspan = _totalColumn;
            _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPcell.Border = 0;
            _pdfPcell.BackgroundColor = BaseColor.WHITE;
            _pdfPcell.ExtraParagraphSpace = 0;
            _pdfPtable.AddCell(_pdfPcell);
            _pdfPtable.CompleteRow();

            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _pdfPcell = new PdfPCell(new Phrase("Sales List", _fontStyle));
            _pdfPcell.Colspan = _totalColumn;
            _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPcell.Border = 0;
            _pdfPcell.BackgroundColor = BaseColor.WHITE;
            _pdfPcell.ExtraParagraphSpace = 0;
            _pdfPtable.AddCell(_pdfPcell);
            _pdfPtable.CompleteRow();

        }

        private void ReportBody()
        {
            #region Table header

            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _pdfPcell = new PdfPCell(new Phrase("Column 1", _fontStyle));
            _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPcell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPcell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPtable.AddCell(_pdfPcell);

            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _pdfPcell = new PdfPCell(new Phrase("Column 2", _fontStyle));
            _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPcell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPcell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPtable.AddCell(_pdfPcell);

            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _pdfPcell = new PdfPCell(new Phrase("Column 3", _fontStyle));
            _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPcell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPcell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPtable.AddCell(_pdfPcell);
            _pdfPtable.CompleteRow();
            #endregion

            #region Table Body
            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            int orderdetailID = 1;
            foreach (SaleDetail saleDetail in _saleDetails)
            {
                _pdfPcell = new PdfPCell(new Phrase(orderdetailID++.ToString(), _fontStyle));
                _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPcell.BackgroundColor = BaseColor.WHITE;
                _pdfPtable.AddCell(_pdfPcell);

                _pdfPcell = new PdfPCell(new Phrase(saleDetail.SaleID.ToString(), _fontStyle));
                _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPcell.BackgroundColor = BaseColor.WHITE;
                _pdfPtable.AddCell(_pdfPcell);

                //_pdfPcell = new PdfPCell(new Phrase(saleDetail.ProductID.ToString(), _fontStyle)) ;
                _pdfPcell = new PdfPCell(new Phrase(saleDetail.Product.ProductName, _fontStyle));
                _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPcell.BackgroundColor = BaseColor.WHITE;
                _pdfPtable.AddCell(_pdfPcell);
                _pdfPtable.CompleteRow();
            }
            #endregion
        }







        public byte[] PrepareSalesReport(List<SaleViewMOdel> sale)
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
            _pdfPtable.SetWidths(new float[] { 20f, 50f, 50f, 50f, 50f, 50f });
            #endregion

            this.SalesReportHeader();
            this.SalesReportBody();
            _pdfPtable.HeaderRows = 2;
            _document.Add(_pdfPtable);
            _document.Close();
            return _memoryStream.ToArray();


        }
        private void SalesReportHeader()
        {
            _fontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
            _pdfPcell = new PdfPCell(new Phrase("Sales Report", _fontStyle));
            _pdfPcell.Colspan = _totalColumn2;
            _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPcell.Border = 0;
            _pdfPcell.BackgroundColor = BaseColor.WHITE;
            _pdfPcell.ExtraParagraphSpace = 0;
            _pdfPtable.AddCell(_pdfPcell);
            _pdfPtable.CompleteRow();

            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _pdfPcell = new PdfPCell(new Phrase("Sales List", _fontStyle));
            _pdfPcell.Colspan = _totalColumn2;
            _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPcell.Border = 0;
            _pdfPcell.BackgroundColor = BaseColor.WHITE;
            _pdfPcell.ExtraParagraphSpace = 0;
            _pdfPtable.AddCell(_pdfPcell);
            _pdfPtable.CompleteRow();

        }

        private void SalesReportBody()
        {
            #region Table header

            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _pdfPcell = new PdfPCell(new Phrase("Column 1", _fontStyle));
            _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPcell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPcell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPtable.AddCell(_pdfPcell);

            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _pdfPcell = new PdfPCell(new Phrase("Column 2", _fontStyle));
            _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPcell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPcell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPtable.AddCell(_pdfPcell);

            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _pdfPcell = new PdfPCell(new Phrase("Column 3", _fontStyle));
            _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPcell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPcell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPtable.AddCell(_pdfPcell);
            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _pdfPcell = new PdfPCell(new Phrase("Column 4", _fontStyle));
            _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPcell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPcell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPtable.AddCell(_pdfPcell);

            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _pdfPcell = new PdfPCell(new Phrase("Column 5", _fontStyle));
            _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPcell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPcell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPtable.AddCell(_pdfPcell);

            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            _pdfPcell = new PdfPCell(new Phrase("Column 6", _fontStyle));
            _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPcell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPcell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfPtable.AddCell(_pdfPcell);
            _pdfPtable.CompleteRow();
            #endregion

            #region Table Body
            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            int orderdetailID = 1;
            foreach (SaleViewMOdel sale in _sales)
            {
                _pdfPcell = new PdfPCell(new Phrase(sale.SaleID.ToString(), _fontStyle));
                _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPcell.BackgroundColor = BaseColor.WHITE;
                _pdfPtable.AddCell(_pdfPcell);

                _pdfPcell = new PdfPCell(new Phrase(sale.Customer.ToString(), _fontStyle));
                //_pdfPcell = new PdfPCell(new Phrase(sale.User.FirstName, _fontStyle));
                _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPcell.BackgroundColor = BaseColor.WHITE;
                _pdfPtable.AddCell(_pdfPcell);

                //_pdfPcell = new PdfPCell(new Phrase(saleDetail.ProductID.ToString(), _fontStyle)) ;
                _pdfPcell = new PdfPCell(new Phrase(sale.OrderNumber, _fontStyle));
                _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPcell.BackgroundColor = BaseColor.WHITE;
                _pdfPtable.AddCell(_pdfPcell);

                _pdfPcell = new PdfPCell(new Phrase(sale.FinalTotal.ToString(), _fontStyle));
                _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPcell.BackgroundColor = BaseColor.WHITE;
                _pdfPtable.AddCell(_pdfPcell);

                _pdfPcell = new PdfPCell(new Phrase(sale.DiscountType.ToString(), _fontStyle));
                _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPcell.BackgroundColor = BaseColor.WHITE;
                _pdfPtable.AddCell(_pdfPcell);

                //_pdfPcell = new PdfPCell(new Phrase(saleDetail.ProductID.ToString(), _fontStyle)) ;
                _pdfPcell = new PdfPCell(new Phrase(sale.DiscountedPrice.ToString(), _fontStyle));
                _pdfPcell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPcell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPcell.BackgroundColor = BaseColor.WHITE;
                _pdfPtable.AddCell(_pdfPcell);
                _pdfPtable.CompleteRow();
            }
            #endregion
        }

    }
}