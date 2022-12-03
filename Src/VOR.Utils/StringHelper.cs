using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace VOR.Utils
{
    public static class StringHelper
    {
        public static string ToJSFormat(this string s)
        {
            if (s != null)
                return s.Replace("'", @"\'");
            return s;
        }

        public static string RemoveSpecials(string str)
        {
            string strTemp = str.Trim().ToUpper();

            Regex regAA = new Regex("[Ã|À|Â|Ä|Á|Å]");
            Regex regEE = new Regex("[É|È|Ê|Ë]");
            Regex regII = new Regex("[Í|Ì|Î|Ï]");
            Regex regOO = new Regex("[Õ|Ó|Ò|Ô|Ö]");
            Regex regUU = new Regex("[Ü|Ú|Ù|Û]");
            Regex regSpecial = new Regex("[-_*+/ '#~&0-9]");
            strTemp = regAA.Replace(strTemp, "A");
            strTemp = regEE.Replace(strTemp, "E");
            strTemp = regII.Replace(strTemp, "I");
            strTemp = regOO.Replace(strTemp, "O");
            strTemp = regUU.Replace(strTemp, "U");
            strTemp = regSpecial.Replace(strTemp, "");
            return strTemp;
        }

        public static string RemoveSpecialsKeepSpaceAndComa(string str)
        {
            string strTemp = string.Empty;
            if (!String.IsNullOrEmpty(str))
            {
                strTemp = str.Trim().ToUpper();

                Regex regAA = new Regex("[Ã|À|Â|Ä|Á|Å]");
                Regex regEE = new Regex("[É|È|Ê|Ë]");
                Regex regII = new Regex("[Í|Ì|Î|Ï]");
                Regex regOO = new Regex("[Õ|Ó|Ò|Ô|Ö]");
                Regex regUU = new Regex("[Ü|Ú|Ù|Û]");
                strTemp = regAA.Replace(strTemp, "A");
                strTemp = regEE.Replace(strTemp, "E");
                strTemp = regII.Replace(strTemp, "I");
                strTemp = regOO.Replace(strTemp, "O");
                strTemp = regUU.Replace(strTemp, "U");

                Regex regSpecial2 = new Regex(@"[^A-Z^'^-^ ]+");
                strTemp = strTemp.Replace("Ç", "C");
                strTemp = strTemp.Replace("-", " ");
                strTemp = regSpecial2.Replace(strTemp, "");
            }

            return strTemp;
        }

        public static string HtmlSubstring(string html, int maxlength)
        {
            //initialize regular expressions
            string htmltag = "</?\\w+((\\s+\\w+(\\s*=\\s*(?:\".*?\"|'.*?'|[^'\">\\s]+))?)+\\s*|\\s*)/?>";
            string emptytags = "<(\\w+)((\\s+\\w+(\\s*=\\s*(?:\".*?\"|'.*?'|[^'\">\\s]+))?)+\\s*|\\s*)/?></\\1>";

            //match all html start and end tags, otherwise get each character one by one..
            var expression = new Regex(string.Format("({0})|(.?)", htmltag));
            MatchCollection matches = expression.Matches(html);

            int i = 0;
            StringBuilder content = new StringBuilder();
            foreach (Match match in matches)
            {
                if (match.Value.Length == 1
                    && i < maxlength)
                {
                    content.Append(match.Value);
                    i++;
                }
                //the match contains a tag
                else if (match.Value.Length > 1)
                    content.Append(match.Value);
            }

            return Regex.Replace(content.ToString(), emptytags, string.Empty);
        }

        public static List<string> retrieveLetterPermutationCase(string originalText)
        {
            List<string> results = new List<string>();
            if (!string.IsNullOrEmpty(originalText))
            {
                char[] caracters = originalText.ToCharArray();
                char prevC = new char();
                for (int i = 0; i < caracters.Length - 1; ++i)
                {
                    string similarText = string.Empty;
                    for (int j = 0; j < caracters.Length; ++j)
                    {
                        if (j == i)
                            prevC = caracters[j];
                        else if ((i + 1) == j)
                        {
                            similarText += caracters[j];
                            similarText += prevC;
                        }
                        else
                            similarText += caracters[j];
                    }
                    results.Add(similarText);
                }
            }
            return results;
        }

        public static string DeleteAccent(String source)
        {
            try
            {
                byte[] dest = Encoding.GetEncoding(1251).GetBytes(source);
                return (Encoding.ASCII.GetString(dest));
            }
            catch
            {
                return source;
            }
        }

        public static string TraiteSexe(string sexe)
        {
            string ret = String.Empty;

            sexe = sexe.ToLower().Replace(".", ""); //M, MLLE, MME

            if (sexe == "m")
            {
                ret = "M";
            }
            else if (sexe == "miss" || sexe == "mlle")
            {
                ret = "MLLE";
            }
            else if (sexe == "mme" || sexe == "f")
            {
                ret = "MME";
            }

            return ret;
        }

        public static string SplitCamelCase(this string str)
        {
            return Regex.Replace(
                Regex.Replace(
                    str,
                    @"(\P{Ll})(\P{Ll}\p{Ll})",
                    "$1 $2"
                ),
                @"(\p{Ll})(\P{Ll})",
                "$1 $2"
            );
        }

        // Convert the string to Pascal case.
        public static string ToPascalCase(this string str)
        {
            str = RemoveSpecialsKeepSpaceAndComa(str).ToLower();

            // If there are 0 or 1 characters, just return the string.
            if (str == null) return str;
            if (str.Length < 2) return str.ToUpper();

            // Split the string into words.
            string[] words = str.Split(
                new char[] { },
                StringSplitOptions.RemoveEmptyEntries);

            // Combine the words.
            string result = "";
            foreach (string word in words)
            {
                result +=
                    word.Substring(0, 1).ToUpper() +
                    word.Substring(1);
            }

            return result;
        }
    }
}