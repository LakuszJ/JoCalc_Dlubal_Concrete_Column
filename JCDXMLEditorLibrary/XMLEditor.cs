using System.Linq;
using System.Xml.Linq;

namespace JCDXMLEditorLibrary
{
    public class XMLEditor
    {

        //CANCELED BECOUSE OF MASTERING .CSV IMPORTER





        //    private static string filtrName = "JC_";

        //    public static Dictionary<int, Dictionary<int, List<InternalForcesMemberSingleItem>>> GenereteDicOfForcesForAllMembers(/*string urlXMLLocation*/)
        //    { 
        //        XDocument xDoc;
        //        // UNCOMMENT FINALLY
        //        xDoc = XDocument.Load(@"C:\Users\ljoze\Desktop\Dlubal_no_3.xml");
        //        //xDoc = XDocument.Load(urlXMLLocation);

        //        var tempRes0 = xDoc.Descendants("load_combination");

        //        Dictionary<int, Dictionary<int, List<InternalForcesMemberSingleItem>>> dicOfInternalForecsForAllMembers_KeyMemberJCNo = new Dictionary<int, Dictionary<int, List<InternalForcesMemberSingleItem>>>();
        //        Dictionary<int, List<InternalForcesMemberSingleItem>> dicOfInternalForecsForSinMember_KeyCombinationNo;

        //        foreach (var item in tempRes0)
        //        {
        //            var combinationNo = int.Parse(item.Element("no").Value);

        //            var tempRes1 = item.Descendants("E_MODEL_MEMBERS_INTERNAL_FORCES");
        //            var tempRes2 = tempRes1.Descendants("item");

        //            foreach (var item2 in tempRes2)
        //            {

        //                if (item2.Element("specification") == null)
        //                {
        //                    continue;
        //                }
        //                InternalForcesMemberSingleItem iFMSI = new InternalForcesMemberSingleItem(item2);


        //                var spec = item2.Element("specification").Value;
        //                var groupJCNoString = StringEditor.ExtractStringAreaBetweenPhrases(spec, filtrName, "|", filtrName.Length);

        //                if (groupJCNoString == "" || groupJCNoString == null)
        //                {
        //                    continue;
        //                }

        //                var groupJCNo = int.Parse(groupJCNoString);
        //                iFMSI.speccyficationJC = groupJCNo;

        //                if (!dicOfInternalForecsForAllMembers_KeyMemberJCNo.ContainsKey(groupJCNo))
        //                {
        //                    dicOfInternalForecsForSinMember_KeyCombinationNo = new Dictionary<int, List<InternalForcesMemberSingleItem>>();

        //                    List<InternalForcesMemberSingleItem> listOfInternalFocesForSinleMember = new List<InternalForcesMemberSingleItem>();
        //                    dicOfInternalForecsForSinMember_KeyCombinationNo.Add(combinationNo, listOfInternalFocesForSinleMember);
        //                    dicOfInternalForecsForSinMember_KeyCombinationNo[combinationNo].Add(iFMSI);

        //                    dicOfInternalForecsForAllMembers_KeyMemberJCNo.Add(groupJCNo, dicOfInternalForecsForSinMember_KeyCombinationNo);
        //                }
        //                else // (dicOfInternalForecsForAllMembers_KeyMemberJCNo.ContainsKey(groupJCNo))
        //                {
        //                    if (!dicOfInternalForecsForAllMembers_KeyMemberJCNo[groupJCNo].ContainsKey(combinationNo))
        //                    {
        //                        List<InternalForcesMemberSingleItem> listOfInternalFocesForSinleMember = new List<InternalForcesMemberSingleItem>();

        //                        dicOfInternalForecsForAllMembers_KeyMemberJCNo[groupJCNo].Add(combinationNo, listOfInternalFocesForSinleMember);
        //                        dicOfInternalForecsForAllMembers_KeyMemberJCNo[groupJCNo][combinationNo].Add(iFMSI);

        //                    }
        //                    else // (dicOfInternalForecsForAllMembers_KeyMemberJCNo[groupJCNo].ContainsKey(combinationNo))
        //                    {
        //                        dicOfInternalForecsForAllMembers_KeyMemberJCNo[groupJCNo][combinationNo].Add(iFMSI);
        //                    }
        //                }
        //                XNode tempXNode = dicOfInternalForecsForAllMembers_KeyMemberJCNo[groupJCNo][combinationNo].Last().singleItem.FirstNode;
        //            }
        //        }
        //        return dicOfInternalForecsForAllMembers_KeyMemberJCNo;
        //    }

        //    public static void CleanDicOfForcesForAllMemmbersFromUnnecessaryItems(Dictionary<int, Dictionary<int, List<InternalForcesMemberSingleItem>>> dicLvl0)
        //    {
        //        foreach (var item in dicLvl0)
        //        {
        //            foreach (var item2 in item.Value)
        //            {
        //                int temp1Count = 0;
        //                int temp2Count = 0;
        //                foreach (var item3 in item2.Value)
        //                {
        //                    temp1Count++;

        //                    if (item3.singleItem.Element("node_number") != null)
        //                    {
        //                        temp2Count++;
        //                    }

        //                    if (temp2Count == 2)
        //                    {
        //                        break;
        //                    }
        //                }
        //                item2.Value.RemoveRange(temp1Count - 1, item2.Value.Count - temp1Count - 1);
        //            }
        //        }
        //    }
        //}


        //public class InternalForcesMemberSingleItem
        //{
        //    public int speccyficationJC { get; internal set; }
        //    public XElement singleItem { get; private set; }


        //    public InternalForcesMemberSingleItem(XElement singleItem)
        //    {
        //        this.singleItem = singleItem;
        //    }
        //    public InternalForcesMemberSingleItem(XElement singleItem, int speccyficationJC) : base()
        //    {
        //        this.speccyficationJC = speccyficationJC;
        //    }
        //}

        //internal static class StringEditor
        //{
        //    public static string ExtractStringAreaBetweenPhrases(string sourseString, string phraseStart, string phraseEnd, int startOffset = 0, int endOffset = 0)
        //    {
        //        if (sourseString.IndexOf(phraseStart) == -1)
        //        {
        //            return null;
        //        }
        //        else
        //        {
        //            var indexOfStartArea = sourseString.IndexOf(phraseStart) + startOffset;

        //            if (phraseEnd == "UntilEnd")
        //            {
        //                string areaString = sourseString.Substring(indexOfStartArea).Trim();
        //                return areaString;
        //            }
        //            else
        //            {
        //                var indexOfEndArea = sourseString.IndexOf(phraseEnd, indexOfStartArea) + endOffset;
        //                if (sourseString.IndexOf(phraseStart) == -1 || sourseString.IndexOf(phraseEnd, indexOfStartArea) == -1)
        //                {
        //                    string areaString = sourseString.Substring(indexOfStartArea).Trim();
        //                    return areaString;
        //                    //return null;
        //                }
        //                else
        //                {
        //                    string areaString = sourseString.Substring(indexOfStartArea, indexOfEndArea - indexOfStartArea).Trim();
        //                    return areaString;
        //                }
        //            }
        //        }
        //    }
    }
}