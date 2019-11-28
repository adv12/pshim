using System;
using System.Reflection;
using System.Text.RegularExpressions;
using SixLabors.ImageSharp.PixelFormats;

namespace PShim
{
    public class PShimUtil
    {
        public static Rgba32 ParseColor(string color)
        {
            if (color == null)
            {
                return new Rgba32(0, 0, 0);
            }
            if (Regex.IsMatch(color.ToUpper(), @"#(\d|[ABCDEF])+"))
            {
                return Rgba32.FromHex(color);
            }
            FieldInfo field = typeof(Rgba32).GetField(color, BindingFlags.Static | BindingFlags.Public);
            if (field != null && field.FieldType == typeof(Rgba32))
            {
                return (Rgba32)field.GetValue(null);
            }
            return new Rgba32(0, 0, 0);
        }
    }
}
