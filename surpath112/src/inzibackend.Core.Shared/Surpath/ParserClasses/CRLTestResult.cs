using System;
using System.Collections.Generic;
using System.Text;

namespace inzibackend.Surpath.ParserClasses
{
    public class CRLTestResult
    {
        public string TestName { get; set; }
        public string Result { get; set; }
        public string Status { get; set; }
        public string CutoffOrExpectedValues { get; set; }
    }
}
