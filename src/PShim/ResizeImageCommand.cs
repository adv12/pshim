using System;
using System.Management.Automation;
using SixLabors.ImageSharp;
using SixLabors.Primitives;
using SixLabors.ImageSharp.Processing;

namespace PShim
{
    [Cmdlet("Resize", "Image", SupportsShouldProcess = true)]
    public class ResizeImageCommand : FileImageCmdlet
    {

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public int? Width { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public int? Height { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public double? Factor { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public double? WidthFactor { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public double? HeightFactor { get; set; }

        [Parameter]
        public SwitchParameter Proportional { get; set; }

        protected override void ProcessRecord()
        {
            if (!ShouldProcess(FileImage.FileInfo.ToString(), "Resize Image"))
            {
                return;
            }
            Image image = FileImage.Image;
            int width = image.Width;
            int height = image.Height;
            if (Width.HasValue || Height.HasValue)
            {
                if (Width.HasValue)
                {
                    width = Width.Value;
                    if (!Height.HasValue)
                    {
                        if (Proportional)
                        {
                            double factor = (double)width / image.Width;
                            height = (int)Math.Round(image.Height * factor);
                        }
                        else if (HeightFactor.HasValue)
                        {
                            height = (int)Math.Round(image.Height * HeightFactor.Value);
                        }
                    }
                }
                if (Height.HasValue)
                {
                    height = Height.Value;
                    if (!Width.HasValue)
                    {
                        if (Proportional)
                        {
                            double factor = (double)height / image.Height;
                            width = (int)Math.Round(image.Width * factor);
                        }
                        else if (WidthFactor.HasValue)
                        {
                            width = (int)Math.Round(image.Width * WidthFactor.Value);
                        }
                    }
                }
            }
            else if (Factor.HasValue)
            {
                width = (int)Math.Round(image.Width * Factor.Value);
                height = (int)Math.Round(image.Height * Factor.Value);
            }
            else if (WidthFactor.HasValue || HeightFactor.HasValue)
            {
                if (WidthFactor.HasValue)
                {
                    width = (int)Math.Round(image.Width * WidthFactor.Value);
                }
                if (HeightFactor.HasValue)
                {
                    height = (int)Math.Round(image.Height * HeightFactor.Value);
                }
            }
            Size size = new Size(width, height);

            image.Mutate(im => im.Resize(size));
            WriteObject(FileImage);
        }
    }
}
