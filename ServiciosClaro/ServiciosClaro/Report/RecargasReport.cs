using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;



namespace ServiciosClaro.Report
{
    public class RecargasReport
    {
        #region Declaration

        int _totalColumn = 4;
        Document _document;
        Font _fontStyle;
        PdfPTable _pdfTable = new PdfPTable(4);
        PdfPCell pdfPCell;
        MemoryStream memoryStream = new MemoryStream();
        List<Recargas> _recargas = new List<Recargas>();
        #endregion

        public byte[] PrepareReport(List<Recargas> recargas)
        {
            _recargas = recargas;

            #region
            _document = new Document(PageSize.A4,0f,0f,0f,0f);
            _document.SetPageSize(PageSize.A4);
            _document.SetMargins(20f,20f,20f,20f);
            _pdfTable.WidthPercentage = 100;
            _pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_document, memoryStream);
            _document.Open();
            _pdfTable.SetWidths(new float[] {20f,150f,100f,100f });
            #endregion

            this.ReportHeader();
            this.ReportBody();
            _pdfTable.HeaderRows = 2;
            _document.Add(_pdfTable);
            _document.Close();
            return memoryStream.ToArray();

        }

        private void ReportBody()
        {

            #region Table header

            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            pdfPCell = new PdfPCell(new Phrase("Id", _fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfTable.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Lugar", _fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfTable.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Precio", _fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfTable.AddCell(pdfPCell);

            pdfPCell = new PdfPCell(new Phrase("Celular", _fontStyle));
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfTable.AddCell(pdfPCell);

            #endregion

            #region Table Body

            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            int SerialNumber = 1;

            foreach (Recargas recargas in _recargas) {
                pdfPCell = new PdfPCell(new Phrase(recargas.Id.ToString(), _fontStyle));
                pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfTable.AddCell(pdfPCell);

                pdfPCell = new PdfPCell(new Phrase(recargas.Lugar, _fontStyle));
                pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfTable.AddCell(pdfPCell);

                pdfPCell = new PdfPCell(new Phrase(recargas.Precio.ToString(), _fontStyle));
                pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfTable.AddCell(pdfPCell);

                pdfPCell = new PdfPCell(new Phrase(recargas.Celular, _fontStyle));
                pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfPCell.BackgroundColor = BaseColor.WHITE;
                _pdfTable.AddCell(pdfPCell);
                _pdfTable.CompleteRow();

            }
            

            #endregion


        }

        private void ReportHeader()
        {
            _fontStyle = FontFactory.GetFont("Tahoma",11f,1);
            pdfPCell = new PdfPCell(new Phrase("Informe Recarga", _fontStyle));
            pdfPCell.Colspan = _totalColumn;
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.Border = 0;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(pdfPCell);
            _pdfTable.CompleteRow();


            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            pdfPCell = new PdfPCell(new Phrase("Recarga", _fontStyle));
            pdfPCell.Colspan = _totalColumn;
            pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfPCell.Border = 0;
            pdfPCell.BackgroundColor = BaseColor.WHITE;
            pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(pdfPCell);
            _pdfTable.CompleteRow();

        }
    }
}