using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCDlubalCSVForcesGeneratorLibrary
{
    public class GlobalInfomationAboutReadedModel
    {
        public DateTime readTime { get;  set; }
        public string pathCSVFiles { get;  set; }
        public Dictionary<string, string> combinationsCSVFileNames { get;  set; }
        public CultureInfo appicationRFEMCuluture { get;  set; }

        public static string specificChars = ";;;;;;;;;;;\r\n";

        public string totalTimeOfEditintFiles { get; set; }
    }
}
