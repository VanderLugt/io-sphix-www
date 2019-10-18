using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Sphix.Utility
{
   public static class Urlhelper
    {
        public static string GenerateSeoFriendlyURL(string Title)
        {
            string phrase = string.Format("{0}",  Title);

            string strHolder = RemoveAccent(phrase).ToLower();
            strHolder = Regex.Replace(strHolder, @"&", " and ");
            strHolder = Regex.Replace(strHolder, @"@", " at ");
            //strHolder = Regex.Replace(Title, @"@", " at ");
            // invalid chars           
            strHolder = Regex.Replace(strHolder, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            strHolder = Regex.Replace(strHolder, @"\s+", " ").Trim();
            // cut and trim 
            strHolder = strHolder.Substring(0, strHolder.Length <= 45 ? strHolder.Length : 45).Trim();
            strHolder = Regex.Replace(strHolder, @"\s", "-"); // hyphens   
            return strHolder;
        }

        private static string RemoveAccent(string text)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(text);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }
    }
}
