using System;
using System.Collections.Generic;
using System.Text;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace inzibackend.Surpath.ParserClasses
{
    public class PDFToTextConverter
    {
        //public static string ConvertPdfToText(string pdfFilePath)
        //{
        //    StringBuilder text = new StringBuilder();

        //    using (PdfDocument document = PdfDocument.Open(pdfFilePath))
        //    {
        //        foreach (Page page in document.GetPages())
        //        {
        //            text.AppendLine(page.Text);
        //        }
        //    }

        //    return text.ToString();
        //}

        public static string ConvertPdfToText(string pdfFilePath)
        {
            StringBuilder text = new StringBuilder();

            using (PdfDocument document = PdfDocument.Open(pdfFilePath))
            {
                foreach (Page page in document.GetPages())
                {
                    var words = page.GetWords();
                    float? lastBottom = null;
                    float? lastRight = null;

                    foreach (var word in words)
                    {
                        if (lastBottom.HasValue)
                        {
                            // If the bottom of this word is significantly different from the last, it's probably a new line
                            if (Math.Abs(word.BoundingBox.Bottom - lastBottom.Value) > 3)
                            {
                                text.AppendLine();
                            }
                            // If this word starts significantly to the left of where the last word ended, it might be a new section
                            else if (lastRight.HasValue && (word.BoundingBox.Left - lastRight.Value) < -10)
                            {
                                text.AppendLine();
                            }
                            // Otherwise, just add a space between words
                            else if (text.Length > 0 && text[text.Length - 1] != ' ')
                            {
                                text.Append(' ');
                            }
                        }

                        text.Append(word.Text);

                        lastBottom = (float?)word.BoundingBox.Bottom;
                        lastRight = (float?)word.BoundingBox.Right;
                    }

                    // Add an extra newline between pages
                    text.AppendLine();
                }
            }

            return text.ToString();
        }

        public static string ConvertPdfToText(byte[] pdfBytes)
        {
            StringBuilder text = new StringBuilder();

            using (PdfDocument document = PdfDocument.Open(pdfBytes))
            {
                foreach (Page page in document.GetPages())
                {
                    text.AppendLine(page.Text);
                }
            }

            return text.ToString();
        }

    }
}
