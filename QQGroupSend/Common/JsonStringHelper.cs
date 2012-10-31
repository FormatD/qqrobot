using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Format.WebQQ.Common
{
    public static class JsonStringHelper
    {
        public static string ClearJsonString(string jsonString)
        {
            return jsonString.Substring(1, jsonString.Length - 2)
                .Replace(@"""", @"\""")
                .Replace(@"\", @"\\");
        }


    }
}
