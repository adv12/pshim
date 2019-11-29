using System;
using System.Management.Automation;
using SixLabors.ImageSharp;
using SixLabors.Primitives;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

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
        [Alias("Background", "BackgroundColor")]
        public string Color { get; set; }

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
            Rgba32 frameColor = PShimUtil.ParseColor(Color);
            using (Image clone = image.Clone(im => im.Flip(FlipMode.None)))
            {
                try
                {
                    int left = padLeft;
                    int top = padTop;
                    width = Math.Max(width, 1);
                    height = Math.Max(height, 1);
                    image.Mutate(im => im.Fill(frameColor)
                        .Pad(Math.Max(image.Width, width),
                                Math.Max(image.Height, height), frameColor)
                        .Crop(width, height));
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
