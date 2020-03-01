using System;

namespace FtlcCommonLib
{
    public class Constants
    {
        public static string MStartStrRE { get; } = "#\\*";
        public static string MEndStrRE { get; } = "\\*#";
        public static string MStartStr { get; } = "#*";
        public static string MEndStr { get; } = "*#";

        public static string FieldPattern
        {
            get { return $"{MStartStrRE}.*?{MEndStrRE}"; }
        }
    }
}
