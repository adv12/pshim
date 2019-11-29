using System;
using System.Reflection;
using System.Text.RegularExpressions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

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

        public static IBrush GetBrush(Brush brushType, string colorString, string backgroundString)
        {
            Rgba32 rgbaColor = ParseColor(colorString);
            Rgba32 rgbaBackground = ParseColor(backgroundString);
            Color color = new Color(rgbaColor);
            Color background = new Color(rgbaBackground);
            switch (brushType)
            {
                case Brush.Horizontal:
                    return Brushes.Horizontal(color, background);
                case Brush.Vertical:
                    return Brushes.Vertical(color, background);
                case Brush.ForwardDiagonal:
                    return Brushes.ForwardDiagonal(color, background);
                case Brush.BackwardDiagonal:
                    return Brushes.BackwardDiagonal(color, background);
                case Brush.Min:
                    return Brushes.Min(color, background);
                case Brush.Percent10:
                    return Brushes.Percent10(color, background);
                case Brush.Percent20:
                    return Brushes.Percent20(color, background);
                default:
                    return Brushes.Solid(color);
            }
        }
    }
}
