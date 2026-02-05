using System.Collections.Generic;
using System.Linq;

namespace SurpathBackend
{
    /// <summary>
    /// The purpose of this class is to fix the RTF data coming out of the database.
    /// 
    /// The RTFLib doesn't have an import - so if you append the results to an RTF, you get RTF Inception
    /// 
    /// WHich prevents proper rendering.
    /// </summary>
    public static class WTFRTF
    {

        public static string DeInceptionRTFString(string rtf)
        {
            string result = rtf;

            string rtfHeaderSig = @"{\rtf1\ansi";
            // get rid of the enclosing RTF header and trailing closing element
            // make sure it's a problem rtf
            bool badRTF = false;
            // let's get a list by splitting on carriage returns
            List<string> rtfLines = rtf.Split('\n').ToList();
            if (rtfLines.Where(x => x.Contains(rtfHeaderSig) == true).ToList().Count > 1)
            {
                badRTF = true;
            }

            if (badRTF)
            {
                // Get rid of extra RTF opening header:
                int index = result.IndexOf(rtfHeaderSig, result.IndexOf(rtfHeaderSig) + 1);
                
                result = result.Substring(result.IndexOf(rtfHeaderSig, index));
                // now strip off the trailing closing }
                result = result.Remove(result.Length - 1);

            }

            return result;
        }


        /// <summary>
        ///  This attemps to get the native Text out of the RTF
        /// </summary>
        /// <param name="rtf"></param>
        /// <returns></returns>
        public static string DeRtf(string rtf)
        {
            string result = DeInceptionRTFString(rtf);
            string endOfRTFHeader = @"plain\f0\fs20";
            
            // get everything between the header and the end:
            int snipStart = result.IndexOf(endOfRTFHeader) + endOfRTFHeader.Length; // because we want everything after that.
            result = result.Substring(snipStart);
            result = result.Remove(result.Length - 1);
            result = result.Replace(@"\line", "");
            return result;
        }

    }
}