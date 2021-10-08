using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Bakerscraper.Searchers
{
    public class GenericRecipeSearchHelper
    {
        public static string SanitiseString(string originalString)
        {
            return UppercaseFirstLetter(HttpUtility.HtmlDecode(originalString.Trim()));
        }

        private static string UppercaseFirstLetter(string original)
        {
            if (string.IsNullOrEmpty(original))
            {
                return string.Empty;
            }
            return char.ToUpper(original[0]) + original.Substring(1);
        }
    }
}
