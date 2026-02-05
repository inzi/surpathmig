using PdfSharp.Pdf;
using PdfSharp.Pdf.Content;
using PdfSharp.Pdf.Content.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurPath.Business.Backend
{
    //public static class PdfSharpExtensions
    //{
    //    public static IEnumerable<string> ExtractText(this PdfPage page)
    //    {
    //        try
    //        {
    //            var content = ContentReader.ReadContent(page);
    //            var text = content.ExtractText();
    //            return text;
    //        }
    //        catch (Exception)
    //        {

    //            throw;
    //        }
    //    }

    //    public static IEnumerable<string> ExtractText(this CObject cObject)
    //    {
    //        try
    //        {
    //            if (cObject is COperator)
    //            {
    //                var cOperator = cObject as COperator;
    //                if (cOperator.OpCode.Name == OpCodeName.Tj.ToString() ||
    //                    cOperator.OpCode.Name == OpCodeName.TJ.ToString())
    //                {
    //                    foreach (var cOperand in cOperator.Operands)
    //                        foreach (var txt in ExtractText(cOperand))
    //                            yield return txt;
    //                }
    //            }
    //            else if (cObject is CSequence)
    //            {
    //                var cSequence = cObject as CSequence;
    //                foreach (var element in cSequence)
    //                    foreach (var txt in ExtractText(element))
    //                        yield return txt;
    //            }
    //            else if (cObject is CString)
    //            {
    //                var cString = cObject as CString;
    //                yield return cString.Value;
    //            }
    //        }
    //        catch (Exception)
    //        {

    //            throw;
    //        }
    //    }
    //}

    //
    //  PdfTextExtractor.cs
    //
    //  Author:
    //       David Schmitt <david@dasz.at>
    //
    //  Copyright (c) 2013 dasz.at OG
    //
    //  This program is free software: you can redistribute it and/or modify
    //  it under the terms of the GNU General Public License as published by
    //  the Free Software Foundation, either version 3 of the License, or
    //  (at your option) any later version.
    //
    //  This program is distributed in the hope that it will be useful,
    //  but WITHOUT ANY WARRANTY; without even the implied warranty of
    //  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    //  GNU General Public License for more details.
    //
    //  You should have received a copy of the GNU General Public License
    //  along with this program.  If not, see <http://www.gnu.org/licenses/>.

    namespace PdfTextract
    {
        using System;
        using System.Collections.Generic;
        using System.Globalization;
        using System.Linq;
        using System.Text;
        using PdfSharp.Pdf;
        using PdfSharp.Pdf.Content;
        using PdfSharp.Pdf.Content.Objects;
        using PdfSharp.Pdf.IO;
        using Serilog;

        public static class PdfTextExtractor
        {

            public static string GetText(string pdfFileName)
            {
                using (var _document = PdfReader.Open(pdfFileName, PdfDocumentOpenMode.ReadOnly))
                {
                    var result = new StringBuilder();
                    foreach (var page in _document.Pages) //.OfType<PdfPage>())
                    {
                        ExtractText(ContentReader.ReadContent(page), result);
                        result.AppendLine();
                    }
                    return result.ToString();
                }
            }

            public static bool ContainsText(PdfDocument _document, string _search, CultureInfo culture, ILogger _logger = null)
            {
                bool retval = false;

                try
                {
                    var result = new StringBuilder();
                    if (!(_logger == null)) _logger.Debug($"PdfTextExtractor parsing....");
                    foreach (var page in _document.Pages) //.OfType<PdfPage>())
                    {
                        ExtractText(ContentReader.ReadContent(page), result);
                        result.AppendLine();
                    }
                    // return result.ToString().Contains(_search, StringComparison.InvariantCultureIgnoreCase);
                    retval = culture.CompareInfo.IndexOf(result.ToString(), _search, CompareOptions.IgnoreCase) >= 0;
                }
                catch (Exception ex)
                {
                    if (!(_logger == null))
                    {
                        _logger.Error(ex.ToString());
                        if (ex.InnerException != null) _logger.Error(ex.InnerException.ToString());
                        if (ex.StackTrace != null) _logger.Error(ex.StackTrace.ToString());

                    }
                }

                return retval;
            }

            #region CObject Visitor
            private static void ExtractText(CObject obj, StringBuilder target)
            {
                if (obj is CArray)
                    ExtractText((CArray)obj, target);
                else if (obj is CComment)
                    ExtractText((CComment)obj, target);
                else if (obj is CInteger)
                    ExtractText((CInteger)obj, target);
                else if (obj is CName)
                    ExtractText((CName)obj, target);
                else if (obj is CNumber)
                    ExtractText((CNumber)obj, target);
                else if (obj is COperator)
                    ExtractText((COperator)obj, target);
                else if (obj is CReal)
                    ExtractText((CReal)obj, target);
                else if (obj is CSequence)
                    ExtractText((CSequence)obj, target);
                else if (obj is CString)
                    ExtractText((CString)obj, target);
                else
                    throw new NotImplementedException(obj.GetType().AssemblyQualifiedName);
            }

            private static void ExtractText(CArray obj, StringBuilder target)
            {
                foreach (var element in obj)
                {
                    ExtractText(element, target);
                }
            }
            private static void ExtractText(CComment obj, StringBuilder target) { /* nothing */ }
            private static void ExtractText(CInteger obj, StringBuilder target) { /* nothing */ }
            private static void ExtractText(CName obj, StringBuilder target) { /* nothing */ }
            private static void ExtractText(CNumber obj, StringBuilder target) { /* nothing */ }
            private static void ExtractText(COperator obj, StringBuilder target)
            {
                if (obj.OpCode.OpCodeName == OpCodeName.Tj || obj.OpCode.OpCodeName == OpCodeName.TJ)
                {
                    foreach (var element in obj.Operands)
                    {
                        ExtractText(element, target);
                    }
                    target.Append(" ");
                }
            }
            private static void ExtractText(CReal obj, StringBuilder target) { /* nothing */ }
            private static void ExtractText(CSequence obj, StringBuilder target)
            {
                foreach (var element in obj)
                {
                    ExtractText(element, target);
                }
            }
            private static void ExtractText(CString obj, StringBuilder target)
            {
                target.Append(obj.Value);
            }
            #endregion
        }
    }

}
