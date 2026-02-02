using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Linq;


namespace ParserLogFixer
{
    class Program
    {
        public static List<filetofix> filestofix = new List<filetofix>();


        static void Main(string[] args)
        {


            if (args.Length < 1) return;
            string foldername = args[0];
            foreach(string _logfile in Directory.GetFiles(foldername))
            {
                if (_logfile.Contains("HL7Parser"))
                {
                    Console.WriteLine($"Processing file: {_logfile}");
                    procfile(_logfile);
                    Console.WriteLine("");
                    Console.WriteLine("");
                    Console.WriteLine("");
                }
            }

            var _types = filestofix.Select(f => f.filetype).Distinct().ToList();
            foreach (var _tt in _types)
            {
                Console.WriteLine(_tt);
                var _distinctFiles = filestofix.Where(f => f.filetype == _tt).ToList().Select(f => f.filename).Distinct().ToList();
                _distinctFiles.ForEach(f2 =>
                {
                    Console.WriteLine(f2);
                });
            }
        }

        static void procfile(string filename)
        {
            string content = File.ReadAllText(filename);
            //string[] processed = content.Split(">>>>>>>>>>>>>>>>>>>> N E W  F I L E <<<<<<<<<<<<<<<<<<<<".ToArray());
            //Console.WriteLine(processed.Length + "new files");
            string newFile = ">>>>>>>>>>>>>>>>>>>> N E W  F I L E <<<<<<<<<<<<<<<<<<<<";
            string sqlerr = @"MySqlTransaction trans) in C:\Dev\Surpath\SurPath.Data\HL7\HL7ParserDaoInsert.cs:line 916";
            string uptodb = "UPDATED to DB:";

            int _start = 0;

            while (_start > -1)
            {

                var updatemsgindex = content.IndexOf(uptodb, _start);
                if (updatemsgindex < 0) break;
                var updatemsgindexln = content.IndexOf('\r', updatemsgindex);
                var newfilelastindex = content.LastIndexOf(newFile, updatemsgindex);
                var newfilelastindexnl = content.LastIndexOf('\r', newfilelastindex) + 1;
                var FileLog = content.Substring(newfilelastindexnl, updatemsgindexln - newfilelastindexnl);

                var x = FileLog.IndexOf(uptodb) + uptodb.Length;
                //var y = FileLog.IndexOf('\r', x);
                var f = FileLog.Substring(x, FileLog.Length - x);
                if (FileLog.Contains(sqlerr))
                {
                    Console.WriteLine($"{f} needs to be reprocessed");

                    filetofix _f = new filetofix() { filename = f };
                    var u1 = FileLog.IndexOf(uptodb);
                    var u2 = FileLog.LastIndexOf("###") + 3;
                    var t1 = FileLog.Substring(u2, u1 - u2);
                    _f.filetype = t1.Trim();
                    filestofix.Add(_f);

                }
                else
                {
                    //Console.WriteLine($"{f} processed without error");
                }
                _start = updatemsgindex + uptodb.Length;

            }

        }
    }

    public class filetofix
    {
        public string filename { get; set; }
        public string filetype { get; set; }
    }

}
