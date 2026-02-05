using PdfFileWriter;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;

namespace SurPath.Enum
{
    public class PrintPDF
    {
        private PdfDocument Document;
        private Font DefaultFont;
        private Int32 PageNo;
        private Int32 PageCount;
        private Int32 PageSize = 53;
        private String[] Content;

        ////////////////////////////////////////////////////////////////////
        // Create charting examples PDF document
        ////////////////////////////////////////////////////////////////////

        public void SavePDF
                (
                String fileName,
                String[] content
                )
        {
            Content = content;
            PageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Content.Length) / Convert.ToDouble(PageSize)));

            // Step 1: Create empty document
            // Arguments: page width: 8.5”, page height: 11”, Unit of measure: inches
            // Return value: PdfDocument main class
            Document = new PdfDocument(PaperType.A4, false, UnitOfMeasure.Inch, fileName);

            // Debug property
            // By default it is set to false. Use it for debugging only.
            // If this flag is set, PDF objects will not be compressed, font and images will be replaced
            // by text place holder. You can view the file with a text editor but you cannot open it with PDF reader.
            Document.Debug = false;

            // create default font for printing
            DefaultFont = new Font("Consolas", 11.00F, FontStyle.Regular);

            // start page number
            PageNo = 1;

            // create PrintPdfDocument
            PdfPrintDocument Print = new PdfPrintDocument(Document, 300.0);

            // the method that will print one page at a time to PrintDocument
            Print.PrintPage += PrintPage;

            // set margins
            Print.SetMargins(0.25, 0.25, 0.25, 0.25);

            // crop the page image result to reduce PDF file size
            Print.CropRect = new Rectangle(0, 0, 8, 11);

            // initiate the printing process (calling the PrintPage method)
            // after the document is printed, add each page an an image to PDF file.
            Print.AddPagesToPdfDocument();

            // dispose of the PrintDocument object
            Print.Dispose();

            // create the PDF file
            Document.CreateFile();

            // start default PDF reader and display the file
            //Process Proc = new Process();
            //Proc.StartInfo = new ProcessStartInfo(fileName);
            //Proc.Start();

            // exit
            return;
        }

        ////////////////////////////////////////////////////////////////////
        // Print each page of the document to PrintDocument class
        // You can use standard PrintDocument.PrintPage(...) method.
        // NOTE: The graphics origin is top left and Y axis is pointing down.
        // In other words this is not PdfContents printing.
        ////////////////////////////////////////////////////////////////////

        public void PrintPage(object sender, PrintPageEventArgs e)
        {
            // graphics object short cut
            Graphics G = e.Graphics;

            // Set everything to high quality
            G.SmoothingMode = SmoothingMode.None;
            G.InterpolationMode = InterpolationMode.Default;
            G.PixelOffsetMode = PixelOffsetMode.None;
            G.CompositingQuality = CompositingQuality.Default;

            // print area within margins
            Rectangle PrintArea = e.MarginBounds;

            // draw rectangle around print area
            //G.DrawRectangle(Pens.DarkBlue, PrintArea);

            // line height
            Int32 LineHeight = DefaultFont.Height + 1;
            Rectangle TextRect = new Rectangle(PrintArea.X, PrintArea.Y, PrintArea.Width, LineHeight);

            String text = "";

            // print some lines
            for (Int32 LineNo = 1; LineNo <= PageSize && Content.Length >= (PageNo - 1) * PageSize + LineNo; LineNo++)
            {
                text = Content[(PageNo - 1) * PageSize + LineNo - 1];
                G.DrawString(text, DefaultFont, Brushes.Black, TextRect);
                TextRect.Y += LineHeight;
            }

            // move on to next page
            PageNo++;
            e.HasMorePages = PageNo <= PageCount;
            return;
        }
    }
}