using System;
using System.Reflection;
using System.Text.RegularExpressions;
using SixLabors.ImageSharp;
using SixLabors.Primitives;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Metadata;

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

        public static IPen GetPen(Pen penType, float width, string colorString)
        {
            Rgba32 rgbaColor = ParseColor(colorString);
            Color color = new Color(rgbaColor);
            switch (penType)
            {
                case Pen.Dash:
                    return Pens.Dash(color, width);
                case Pen.DashDot:
                    return Pens.DashDot(color, width);
                case Pen.DashDotDot:
                    return Pens.DashDotDot(color, width);
                case Pen.Dot:
                    return Pens.Dot(color, width);
                default:
                    return Pens.Solid(color, width);
            }
        }

        public static PointF GetDpi(Image image)
        {
            ImageMetadata metadata = image.Metadata;
            switch (metadata.ResolutionUnits)
            {
                case PixelResolutionUnit.PixelsPerInch:
                    return new PointF((float)metadata.HorizontalResolution,
                        (float)metadata.VerticalResolution);
                case PixelResolutionUnit.PixelsPerCentimeter:
                    return new PointF((float)(metadata.HorizontalResolution * 2.54),
                        (float)(metadata.VerticalResolution * 2.54));
                case PixelResolutionUnit.PixelsPerMeter:
                    return new PointF((float)(metadata.HorizontalResolution / 100 * 2.54),
                        (float)(metadata.VerticalResolution / 100 * 2.54));
                default:
                    return new PointF(96, 96);
            }
        }
    }
}
