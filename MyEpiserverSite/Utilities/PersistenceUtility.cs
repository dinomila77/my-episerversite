using System;
using System.Collections.Generic;
using System.Text;

namespace MyEpiserverSite.Utilities
{
    public class PersistenceUtility
    {
        public static List<string> TextToFile(string input)
        {
            var builder = new StringBuilder();
            var dateNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss  ");
            builder.AppendLine($"Date: {dateNow}").AppendLine(input);
            var text = builder.ToString();
            var textToFile = new List<string> { text };

            return textToFile;
        }
    }
}