using System;
using System.Globalization;
using System.Management.Automation;
using System.Reflection;
using System.Text.RegularExpressions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace PShim
{
    public static class PShimCmdletExtensions
    {
        public static Rgba32 ParseColor(this Cmdlet cmdlet, string color)
        {
            string originalColor = color;
            if (color == null)
            {
                return new Rgba32(0, 0, 0);
            }
            if (Regex.IsMatch(color.ToUpper(), @"#(\d|[ABCDEF])+"))
            {
                try
                {
                    return Rgba32.FromHex(color);
                }
                catch (Exception ex)
                {
                    ErrorRecord error = new ErrorRecord(ex, "InvalidHexColor",
                        ErrorCategory.InvalidArgument, originalColor);
                    cmdlet.WriteError(error);
                    return Rgba32.Black;
                }
            }
            if (!Regex.IsMatch(color, "[A-Z][A-Za-z]*"))
            {
                TextInfo info = CultureInfo.InvariantCulture.TextInfo;
                color = info.ToTitleCase(color).Replace(" ", "").Replace("_", "");
            }
            FieldInfo field = typeof(Rgba32).GetField(color, BindingFlags.Static | BindingFlags.Public);
            if (field != null && field.FieldType == typeof(Rgba32))
            {
                return (Rgba32)field.GetValue(null);
            }
            cmdlet.WriteWarning($"Invalid color name {originalColor}; defaulting to Black");
            return Rgba32.Black;
        }

        public static IBrush GetBrush(this Cmdlet cmdlet, Brush brushType, string colorString, string backgroundString)
        {
            Rgba32 rgbaColor = cmdlet.ParseColor(colorString);
            Rgba32 rgbaBackground = cmdlet.ParseColor(backgroundString);
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

        public static IPen GetPen(this Cmdlet cmdlet, Pen penType, float width, string colorString)
        {
            Rgba32 rgbaColor = cmdlet.ParseColor(colorString);
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

        public static PointF GetDpi(this Cmdlet cmdlet, Image image)
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
                    return new PointF(72, 72);
            }
        }
    }
}
