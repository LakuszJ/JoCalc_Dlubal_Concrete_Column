using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace JCDlubalCSVForcesGeneratorLibrary
{

    public class CSVEditorAndForceGenerator
    {

        GlobalInfomationAboutReadedModel gIABRM { get; set; }

        public CSVEditorAndForceGenerator(GlobalInfomationAboutReadedModel gIABRM)
        {
            ////      JSON FOR TESTS

            //string testString =
            //"{\"readTime\":\"0001-01-01T00:00:00\"," +
            //"\"CSVFilePath\":\"C:\\\\Users\\\\ljoze\\\\source\\\\repos\\\\JoCalcDlubalClient\\\\JoCalcDlubalClientConsoleUI\\\\bin\\\\Debug\\\\net6.0\\\\CSV_Project_Data\\\\Model_1\"," +
            //"\"newCombinationsCSVFileNames\":{\"CO1\":\"CO1_static_analysis_members_internal_forces.csv\",\"CO2\":\"CO2_static_analysis_members_internal_forces.csv\",\"CO3\":\"CO3_static_analysis_members_internal_forces.csv\",\"CO4\":\"CO4_static_analysis_members_internal_forces.csv\",\"CO5\":\"CO5_static_analysis_members_internal_forces.csv\",\"CO6\":\"CO6_static_analysis_members_internal_forces.csv\",\"CO7\":\"CO7_static_analysis_members_internal_forces.csv\"}," +
            //"\"appicationRFEMCuluture\":\"en-GB\"}"
            //;

            //this.gIABRM = JsonConvert.DeserializeObject<GlobalInfomationAboutReadedModel>(testString); // change for jsonPath
            ////


            this.gIABRM = gIABRM;
        }

        private static string filtrName = "JC_";


        public Dictionary<int, Dictionary<int, List<InternalForcesMemberSingleItem>>> GenereteDicOfForcesForAllMembers()
        {
            string specificChars = GlobalInfomationAboutReadedModel.specificChars;

            Dictionary<int, Dictionary<int, List<InternalForcesMemberSingleItem>>> dicOfInternalForecsForAllMembers_KeyMemberNo = new Dictionary<int, Dictionary<int, List<InternalForcesMemberSingleItem>>>();
            Dictionary<int, List<InternalForcesMemberSingleItem>> dicOfInternalForecsForSinMember_KeyCombinationNo;

            foreach (var item in gIABRM.newCombinationsCSVFileNames)
            {
                int coNo = int.Parse(item.Key.Remove(0, 2));
                string doc = ToolForManipulationFiles.ReadTxtFile(gIABRM.CSVFilePath + '/' + item.Value);
                var sSplit1 = doc.Split(specificChars);

                foreach (var sSplit1Item in sSplit1)
                {
                    int? memberNo = null; 
                    var sSplit2 = sSplit1Item.Trim().Split("\r\n");

                    for (int i = 0; i < sSplit2.Length; i++)
                    {
                        InternalForcesMemberSingleItem iFMSI = new InternalForcesMemberSingleItem();

                        var sSplit3 = sSplit2[i].Split(';');
                        iFMSI.memberControlNo = int.Parse(sSplit3[0]);
                        iFMSI.location = (decimal)Math.Round(double.Parse(ToolForStringEdition.ReplaceSeparatorWithDefaultDecimalSeparator(sSplit3[2])),3); 
                        iFMSI.n = (int)(double.Parse(ToolForStringEdition.ReplaceSeparatorWithDefaultDecimalSeparator(sSplit3[5])) * 1000);
                        iFMSI.vy = (int)(double.Parse(ToolForStringEdition.ReplaceSeparatorWithDefaultDecimalSeparator(sSplit3[6])) * 1000);
                        iFMSI.vz = (int)(double.Parse(ToolForStringEdition.ReplaceSeparatorWithDefaultDecimalSeparator(sSplit3[7])) * 1000);
                        iFMSI.mt = (int)(double.Parse(ToolForStringEdition.ReplaceSeparatorWithDefaultDecimalSeparator(sSplit3[8])) * 1000);
                        iFMSI.my = (int)(double.Parse(ToolForStringEdition.ReplaceSeparatorWithDefaultDecimalSeparator(sSplit3[9])) * 1000);
                        iFMSI.mz = (int)(double.Parse(ToolForStringEdition.ReplaceSeparatorWithDefaultDecimalSeparator(sSplit3[10])) * 1000);

                        string spec = sSplit2.Last();
                        var groupJCNoString = ToolForStringEdition.ExtractStringAreaBetweenPhrases(spec, filtrName, "UntilEnd", filtrName.Length);
                        if (groupJCNoString == null)
                        { }
                        else
                        {
                            iFMSI.speccyficationJC = int.Parse(groupJCNoString);
                        }

                        if (i==0)
                        {
                            iFMSI.startNode = int.Parse(sSplit3[1]);
                            memberNo = iFMSI.memberControlNo;
                        }

                        if (i == sSplit2.Length-1)
                        {
                            dicOfInternalForecsForAllMembers_KeyMemberNo[iFMSI.memberControlNo][coNo][0].endNode = int.Parse(sSplit3[1]);
                        }

                        if (!dicOfInternalForecsForAllMembers_KeyMemberNo.ContainsKey(memberNo.Value))
                        {
                            dicOfInternalForecsForSinMember_KeyCombinationNo = new Dictionary<int, List<InternalForcesMemberSingleItem>>();

                            List<InternalForcesMemberSingleItem> listOfInternalFocesForSinleMember = new List<InternalForcesMemberSingleItem>();
                            dicOfInternalForecsForSinMember_KeyCombinationNo.Add(coNo, listOfInternalFocesForSinleMember);
                            dicOfInternalForecsForSinMember_KeyCombinationNo[coNo].Add(iFMSI);

                            dicOfInternalForecsForAllMembers_KeyMemberNo.Add(memberNo.Value, dicOfInternalForecsForSinMember_KeyCombinationNo);
                        }
                        else // (dicOfInternalForecsForAllMembers_KeyMemberNo.ContainsKey(memberNo))
                        {
                            if (!dicOfInternalForecsForAllMembers_KeyMemberNo[memberNo.Value].ContainsKey(coNo))
                            {
                                List<InternalForcesMemberSingleItem> listOfInternalFocesForSinleMember = new List<InternalForcesMemberSingleItem>();

                                dicOfInternalForecsForAllMembers_KeyMemberNo[memberNo.Value].Add(coNo, listOfInternalFocesForSinleMember);
                                dicOfInternalForecsForAllMembers_KeyMemberNo[memberNo.Value][coNo].Add(iFMSI);

                            }
                            else // (dicOfInternalForecsForAllMembers_KeyMemberNo[memberNo].ContainsKey(coNo))
                            {
                                dicOfInternalForecsForAllMembers_KeyMemberNo[memberNo.Value][coNo].Add(iFMSI);
                            }
                        }
                    }
                }
            }
            return dicOfInternalForecsForAllMembers_KeyMemberNo;
        }


    }
}
