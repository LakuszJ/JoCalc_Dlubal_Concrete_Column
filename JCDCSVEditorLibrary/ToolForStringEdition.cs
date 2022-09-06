using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCDlubalCSVForcesGeneratorLibrary
{
    public static class ToolForStringEdition
    {
        public static string ExtractStringAreaBetweenPhrases(string sourseString, string phraseStart, string phraseEnd, int startOffset = 0, int endOffset = 0)
        {
            if (sourseString.IndexOf(phraseStart) == -1)
            {
                return null;
            }
            else
            {
                var indexOfStartArea = sourseString.IndexOf(phraseStart) + startOffset;

                if (phraseEnd == "UntilEnd")
                {
                    string areaString = sourseString.Substring(indexOfStartArea).Trim();
                    return areaString;
                }
                else
                {
                    var indexOfEndArea = sourseString.IndexOf(phraseEnd, indexOfStartArea) + endOffset;
                    if (sourseString.IndexOf(phraseStart) == -1 || sourseString.IndexOf(phraseEnd, indexOfStartArea) == -1)
                    {
                        return null;
                    }
                    else
                    {
                        string areaString = sourseString.Substring(indexOfStartArea, indexOfEndArea - indexOfStartArea).Trim();
                        return areaString;
                    }
                }
            }
        }

        public static string RemoveStringAreaBetweenPhrases(string sourseString, string phraseStart, string phraseEnd, int startOffset = 0, int endOffset = 0)
        {
            int indexPhraseStart = sourseString.IndexOf(phraseStart);

            if (indexPhraseStart == -1)
            { return null; }
            else
            {
                var indexOfStartArea = indexPhraseStart + startOffset;

                if (phraseEnd == "UntilEnd")
                {
                    string areaString = sourseString.Remove(indexOfStartArea).Trim();
                    return areaString;
                }
                else
                {
                    int indexPhraseEnd = sourseString.IndexOf(phraseEnd, indexOfStartArea);
                    int indexOfEndArea = indexPhraseEnd + endOffset;
                    if (indexPhraseStart == -1 || indexPhraseEnd == -1)
                    { return null; }
                    else
                    {
                        string areaString = sourseString.Remove(indexOfStartArea, indexOfEndArea - indexOfStartArea).Trim();
                        return areaString;
                    }
                }
            }

        }

        public static readonly char decSeparator = Convert.ToChar(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
        public static string ReplaceSeparatorWithDefaultDecimalSeparator(string s)
        {
            s = s.Replace(',', '.').Replace('.', decSeparator).Trim();
            return s;
        }

    }
}
