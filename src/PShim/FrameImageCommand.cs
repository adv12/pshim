using System;
using System.Collections.Generic;
using System.Management.Automation;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using SixLabors.Shapes;

namespace PShim
{
    [Cmdlet("Frame", "Image", SupportsShouldProcess = true)]
    public class FrameImageCommand : FileImageCmdlet
    {

        [Parameter(ValueFromPipelineByPropertyName = true,
            ValueFromRemainingArguments = true)]
        [ValidateCount(1, 1)]
        public int[] All { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public int? LeftRight { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public int? TopBottom { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public int? Left { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public int? Right { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public int? Top { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public int? Bottom { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public string Color { get; set; } = "Black";

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public string Background { get; set; } = "Black";

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public Brush Brush { get; set; } = Brush.Solid;

        protected override void ProcessRecord()
        {
            if (!ShouldProcess(FileImage.FileInfo.ToString(), "Frame Image"))
            {
                return;
            }
            Image image = FileImage.Image;
            int padLeft = 0;
            int padRight = 0;
            int padTop = 0;
            int padBottom = 0;
            if (All != null && All.Length > 0)
            {
                padLeft = padRight = padTop = padBottom = All[0];
            }
            if (LeftRight.HasValue)
            {
                padLeft = padRight = LeftRight.Value;
            }
            padLeft = Left ?? padLeft;
            padRight = Right ?? padRight;
            if (TopBottom.HasValue)
            {
                padTop = padBottom = TopBottom.Value;
            }
            padTop = Top ?? padTop;
            padBottom = Bottom ?? padBottom;
            int width = image.Width + padLeft + padRight;
            int height = image.Height + padTop + padBottom;
            Rgba32 frameColor = this.ParseColor(Color);
            using (Image clone = image.Clone(im => im.Flip(FlipMode.None)))
            {
                try
                {
                    int left = padLeft;
                    int top = padTop;
                    width = Math.Max(width, 1);
                    height = Math.Max(height, 1);
                    IBrush brush = this.GetBrush(Brush, Color, Background);
                    image.Mutate(im => im.Fill(Rgba32.White)
                        .Pad(Math.Max(image.Width, width),
                                Math.Max(image.Height, height), Rgba32.White)
                        .Crop(width, height));
                    PathBuilder pathBuilder = new PathBuilder();
                    List<PointF> outerPoints = new List<PointF>();
                    outerPoints.Add(new PointF(0, 0));
                    outerPoints.Add(new PointF(image.Width, 0));
                    outerPoints.Add(new PointF(image.Width, image.Height));
                    outerPoints.Add(new PointF(0, image.Height));
                    outerPoints.Add(new PointF(0, 0));
                    padLeft = Math.Max(padLeft, 0);
                    padRight = Math.Max(padRight, 0);
                    padTop = Math.Max(padTop, 0);
                    padBottom = Math.Max(padBottom, 0);
                    List<PointF> innerPoints = new List<PointF>();
                    innerPoints.Add(new PointF(padLeft, padTop));
                    innerPoints.Add(new PointF(image.Width - padRight, padTop));
                    innerPoints.Add(new PointF(image.Width - padRight, image.Height - padBottom));
                    innerPoints.Add(new PointF(padLeft, image.Height - padBottom));
                    innerPoints.Add(new PointF(padLeft, padTop));
                    pathBuilder.StartFigure();
                    pathBuilder.AddLines(outerPoints);
                    pathBuilder.StartFigure();
                    pathBuilder.AddLines(innerPoints);
                    pathBuilder.CloseAllFigures();
                    image.Mutate(im => im.Fill(brush, pathBuilder.Build()));
                    if (left + clone.Width > 0 && top + clone.Height > 0)
                    {
                        image.Mutate(im => im.DrawImage(clone, new Point(left, top), 1));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            WriteObject(FileImage);
        }
    }
}
