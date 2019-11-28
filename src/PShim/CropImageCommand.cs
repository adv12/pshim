using System;
using System.Management.Automation;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace PShim
{
    [Cmdlet("Crop", "Image")]
    public class CropImageCommand : PSCmdlet
    {

        [Parameter(ValueFromPipeline = true)]
        public FileImage FileImage { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public int? Width { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public int? Height { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public int? SubtractWidth { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public int? SubtractHeight { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public AnchorPositionMode Alignment { get; set; } = AnchorPositionMode.Center;

        [Parameter(ValueFromPipelineByPropertyName = true)]
        [Alias("Color", "BackgroundColor")]
        public string Background { get; set; }

        protected override void ProcessRecord()
        {
            if (!ShouldProcess(FileImage.FileInfo.ToString(), "Crop Image"))
            {
                return;
            }
            Image image = FileImage.Image;
            int width = image.Width;
            int height = image.Height;
            if (Width.HasValue)
            {
                width = Width.Value;
            }
            else if (SubtractWidth.HasValue)
            {
                width = image.Width - SubtractWidth.Value;
            }
            if (Height.HasValue)
            {
                height = Height.Value;
            }
            else
            {
                height = image.Height - SubtractHeight.Value;
            }
            if (width > image.Width || height > image.Height)
            {
                int w = Math.Max(width, image.Width);
                int h = Math.Max(height, image.Height);
                ResizeOptions options = new ResizeOptions
                {
                    Position = Alignment,
                    Mode = ResizeMode.BoxPad,
                    Size = new Size(w, h)
                };
                image.Mutate(im => im.Resize(options)
                    .BackgroundColor(PShimUtil.ParseColor(Background)));
            }
            if (width < image.Width || height < image.Height)
            {
                int left = 0;
                int top = 0;
                switch (Alignment)
                {
                    case AnchorPositionMode.Top:
                    case AnchorPositionMode.TopLeft:
                    case AnchorPositionMode.TopRight:
                        top = 0;
                        break;
                    case AnchorPositionMode.Center:
                    case AnchorPositionMode.Left:
                    case AnchorPositionMode.Right:
                        top = (int)Math.Round((image.Height - height) / 2.0);
                        break;
                    case AnchorPositionMode.Bottom:
                    case AnchorPositionMode.BottomLeft:
                    case AnchorPositionMode.BottomRight:
                        top = image.Height - height;
                        break;
                }
                switch (Alignment)
                {
                    case AnchorPositionMode.BottomLeft:
                    case AnchorPositionMode.Left:
                    case AnchorPositionMode.TopLeft:
                        left = 0;
                        break;
                    case AnchorPositionMode.Top:
                    case AnchorPositionMode.Center:
                    case AnchorPositionMode.Bottom:
                        left = (int)Math.Round((image.Width - width) / 2.0);
                        break;
                    case AnchorPositionMode.BottomRight:
                    case AnchorPositionMode.Right:
                    case AnchorPositionMode.TopRight:
                        left = image.Width - width;
                        break;
                }
                image.Mutate(im => im.Crop(new Rectangle(left, top, width, height)));
            }
            WriteObject(FileImage);
        }
    }
}
