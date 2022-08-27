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


        //static public object DeserializeJson(/*string jsonPath)*/)
        //{
        //    //string testString =
        //    //    "{\"readTime\":\"0001-01-01T00:00:00\"," +
        //    //    "\"CSVFilePath\":\"C:\\\\Users\\\\ljoze\\\\source\\\\repos\\\\JoCalcDlubalClient\\\\JoCalcDlubalClientConsoleUI\\\\bin\\\\Debug\\\\net6.0\\\\CSV_Project_Data\\\\Model_1\"," +
        //    //    "\"newCombinationsCSVFileNames\":{\"CO1\":\"CO1_static_analysis_members_internal_forces.csv\",\"CO2\":\"CO2_static_analysis_members_internal_forces.csv\",\"CO3\":\"CO3_static_analysis_members_internal_forces.csv\",\"CO4\":\"CO4_static_analysis_members_internal_forces.csv\",\"CO5\":\"CO5_static_analysis_members_internal_forces.csv\",\"CO6\":\"CO6_static_analysis_members_internal_forces.csv\",\"CO7\":\"CO7_static_analysis_members_internal_forces.csv\"}," +
        //    //    "\"appicationRFEMCuluture\":\"en-GB\"}"
        //    //    ;
        //    //object stObj = JsonConvert.DeserializeObject<>(testString); // change for jsonPath
        //    //return stObj;
        //}

        public static void SaveTxtFile(string path, string source)
        {
            File.WriteAllText(path, source);
            //using (StreamWriter sw = new StreamWriter(path))
            //{
            //    sw.Write(source);
            //    sw.Close();
            //}
        }
    }

}
