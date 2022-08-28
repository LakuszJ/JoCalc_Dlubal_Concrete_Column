using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCDlubalCSVForcesGeneratorLibrary
{
    public static class ToolForManipulationFiles
    {
        public static string ReadTxtFile(string path)
        {
            if (File.Exists(path))
            {
                using (StreamReader streamReader = new StreamReader(path))
                {
                    string readedText = streamReader.ReadToEnd();
                    streamReader.Close();
                    return readedText;
                }
            }
            else
            {
                return "";
            }
        }

        public static void SaveTxtFile(string path, string source)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(source);
                sw.Close();
            }
        }
    }

}
