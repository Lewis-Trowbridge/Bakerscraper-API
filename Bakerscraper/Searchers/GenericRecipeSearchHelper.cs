using System;
using System.Text;
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

        public static string NormaliseString(string originalString)
        {
            return originalString.Normalize().Replace("\u00A0", " ");
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
