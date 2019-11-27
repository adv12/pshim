using System;
using System.Management.Automation;
using SixLabors.ImageSharp;
using SixLabors.Primitives;
using SixLabors.ImageSharp.Processing;

namespace PShim
{
    [Cmdlet("Resize", "Image")]
    public class ResizeImageCommand : PSCmdlet
    {

        [Parameter(ValueFromPipeline = true)]
        public FileImage FileImage { get; set; }

        [Parameter]
        public int? Width { get; set; }

        [Parameter]
        public int? Height { get; set; }

        [Parameter]
        public double? Factor { get; set; }

        [Parameter]
        public double? WidthFactor { get; set; }

        [Parameter]
        public double? HeightFactor { get; set; }

        [Parameter]
        public SwitchParameter Proportional { get; set; }

        protected override void ProcessRecord()
        {
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
            if (ShouldProcess(FileImage.FileInfo.ToString(), "Resize Image"))
            {
                image.Mutate(im => im.Resize(size));
                WriteObject(FileImage);
            }
        }
    }
}
