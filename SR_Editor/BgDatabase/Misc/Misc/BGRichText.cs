/*
<copyright file="BGRichText.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// RichText utilities
    /// </summary>
    public static partial class BGRichText
    {
        public static string DefaultColor = "blue";

        public static string Bold(string text)
        {
            return Quote("b", text);
        }

        public static string Italic(string text)
        {
            return Quote("i", text);
        }

        public static string Size(string text, int size)
        {
            return Quote("size", "" + size, text);
        }

        public static string Color(string text, string color)
        {
            return Quote("color", "" + color, text);
        }

        public static string Highlight(string text, string color)
        {
            if (string.IsNullOrEmpty(color)) color = DefaultColor;
            return Bold(Color(text, color));
        }

        public static string Highlight(string text)
        {
            return Highlight(text, null);
        }

        private static string Quote(string quote, string text)
        {
            return "<" + quote + ">" + text + "</" + quote + ">";
        }

        private static string Quote(string quote, string value, string text)
        {
            return "<" + quote + "=" + value + ">" + text + "</" + quote + ">";
        }
    }
}