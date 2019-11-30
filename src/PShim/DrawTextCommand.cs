using System;
using System.Management.Automation;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace PShim
{
    [Cmdlet("Draw", "Text")]
    public class DrawTextCommand : FileImageCmdlet
    {
        [Parameter(Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromRemainingArguments = true)]
        [ValidateCount(1, int.MaxValue)]
        public string[] Text { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public string FontFamily { get; set; } = "Arial";

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public FontStyle FontStyle { get; set; } = FontStyle.Regular;

        [Parameter(ValueFromPipelineByPropertyName = true)]
        [Alias("FontSize")]
        public float Size { get; set; } = 12;

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public AnchorPositionMode Alignment { get; set; } = AnchorPositionMode.TopLeft;

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public float? PadLeft { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public float? PadRight { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public float? PadTop { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public float? PadBottom { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public float? MaxWidth { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public float? MaxHeight { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        [Alias("FontColor", "FillColor")]
        public string Color { get; set; } = "Black";

        [Parameter(ValueFromPipelineByPropertyName = true)]
        [Alias("FontBackground", "FillBackground")]
        public string Background { get; set; } = "Transparent";

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public Brush Brush { get; set; } = Brush.Solid;

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public Pen Pen { get; set; } = Pen.Solid;

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public float PenWidth { get; set; } = 1;

        [Parameter(ValueFromPipelineByPropertyName = true)]
        [Alias("OutlineColor")]
        public string PenColor { get; set; } = "Transparent";

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public bool Antialias { get; set; } = true;

        [Parameter]
        public SwitchParameter RespectDpi { get; set; }

        protected override void ProcessRecord()
        {
            if (!ShouldProcess(FileImage.FileInfo.ToString(), "Draw Text"))
            {
                return;
            }

            Image image = FileImage.Image;

            float left = 0;
            float top = 0;
            float maxWidth = Math.Min(MaxWidth ?? image.Width,
                image.Width - (PadLeft ?? 0) - (PadRight ?? 0));
            float maxHeight = Math.Min(MaxHeight ?? image.Height,
                image.Height - (PadTop ?? 0) - (PadBottom ?? 0));
            TextGraphicsOptions options = new TextGraphicsOptions(Antialias);
            FontFamily fontFamily = SystemFonts.Find("Arial");
            Font font = new Font(fontFamily, Size, FontStyle);
            string text = string.Join(Environment.NewLine, Text);
            PointF dpi = RespectDpi ? PShimUtil.GetDpi(image) : new PointF(72, 72);
            RendererOptions rendererOptions =
                new RendererOptions(font, dpi.X, dpi.Y);
            SizeF size = TextMeasurer.Measure(text, rendererOptions);
            if (maxWidth < size.Width)
            {
                float newFontSize = font.Size * (maxWidth / size.Width);
                font = new Font(font, newFontSize);
            }
            rendererOptions =
                new RendererOptions(font, dpi.X, dpi.Y);
            size = TextMeasurer.Measure(text, rendererOptions);
            if (maxHeight < size.Height)
            {
                float newFontSize = font.Size * (maxHeight / size.Height);
                font = new Font(font, newFontSize);
            }
            rendererOptions =
               new RendererOptions(font, dpi.X, dpi.Y);
            size = TextMeasurer.Measure(text, rendererOptions);
            font = new Font(font, font.Size * (dpi.X / 72));
            //Console.WriteLine($"{FileImage.FileInfo} ({image.Width}x{image.Height}): DPI({dpi.X}x{dpi.Y}), size({size.Width}x{size.Height})");
            switch (Alignment)
            {
                case AnchorPositionMode.BottomLeft:
                case AnchorPositionMode.Left:
                case AnchorPositionMode.TopLeft:
                    left = PadLeft ?? 0;
                    break;
                case AnchorPositionMode.Bottom:
                case AnchorPositionMode.Center:
                case AnchorPositionMode.Top:
                    left = (image.Width - size.Width) / 2.0f;
                    break;
                case AnchorPositionMode.BottomRight:
                case AnchorPositionMode.Right:
                case AnchorPositionMode.TopRight:
                    left = image.Width - (PadRight ?? 0) - size.Width;
                    break;
            }
            switch (Alignment)
            {
                case AnchorPositionMode.Top:
                case AnchorPositionMode.TopLeft:
                case AnchorPositionMode.TopRight:
                    top = PadTop ?? 0;
                    break;
                case AnchorPositionMode.Left:
                case AnchorPositionMode.Center:
                case AnchorPositionMode.Right:
                    top = (image.Height - size.Height) / 2.0f;
                    break;
                case AnchorPositionMode.Bottom:
                case AnchorPositionMode.BottomLeft:
                case AnchorPositionMode.BottomRight:
                    top = image.Height - (PadBottom ?? 0) - size.Height;
                    break;
            }
            IBrush brush = PShimUtil.GetBrush(Brush, Color, Background);
            IPen pen = PShimUtil.GetPen(Pen, PenWidth, PenColor);
            try
            {
                image.Mutate(im => im.DrawText(options, text, font, brush, pen,
                    new PointF(Math.Max(left, 0), Math.Max(top, 0))));
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex);
            }
            WriteObject(FileImage);
        }
    }
}
