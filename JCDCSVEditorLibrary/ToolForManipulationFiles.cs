using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        public static T1 deserializeObject<T1>(string jsonString)
        {
            T1 newElement = JsonConvert.DeserializeObject<T1>(jsonString);
            return newElement;
        }

        public static Dictionary<int, T1> deserializeOneLevelDic<T1>(string jsonString)
        {
            var values = JsonConvert.DeserializeObject<Dictionary<int, object>>(jsonString);
            var values2 = new Dictionary<int, T1>();
            foreach (KeyValuePair<int, object> d in values)
            {
                if (d.Value is JObject)
                {
                    values2.Add(d.Key, deserializeObject<T1>(d.Value.ToString()));
                }
                else
                {
                    values2.Add(d.Key, (T1)d.Value);
                }
            }
            return values2;
        }


        public static Dictionary<int, Dictionary<int, T1>> deserializeTwoLevelDic<T1>(string jsonString)
        {
            var values2_1 = JsonConvert.DeserializeObject<Dictionary<int, object>>(jsonString);
            var values2_2 = new Dictionary<int, Dictionary<int, T1>>();
            foreach (KeyValuePair<int, object> d2 in values2_1)
            {
                if (d2.Value is JObject)
                {
                    var values3_1 = JsonConvert.DeserializeObject<Dictionary<int, object>>(d2.Value.ToString());
                    var values3_2 = new Dictionary<int, T1>();
                    foreach (KeyValuePair<int, object> d3 in values3_1)
                    {
                        values3_2.Add(d3.Key, deserializeObject<T1>(d3.Value.ToString()));
                    }
                    values2_2.Add(d2.Key, values3_2);
                }
            }
            return values2_2;
        }

    }

}
