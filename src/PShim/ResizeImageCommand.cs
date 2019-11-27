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
        public Image Image { get; set; }

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
            int width = Image.Width;
            int height = Image.Height;
            if (Width.HasValue || Height.HasValue)
            {
                if (Width.HasValue)
                {
                    width = Width.Value;
                    if (!Height.HasValue)
                    {
                        if (Proportional)
                        {
                            double factor = (double)width / Image.Width;
                            height = (int)Math.Round(Image.Height * factor);
                        }
                        else if (HeightFactor.HasValue)
                        {
                            height = (int)Math.Round(Image.Height * HeightFactor.Value);
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
                            double factor = (double)height / Image.Height;
                            width = (int)Math.Round(Image.Width * factor);
                        }
                        else if (WidthFactor.HasValue)
                        {
                            width = (int)Math.Round(Image.Width * WidthFactor.Value);
                        }
                    }
                }
            }
            else if (Factor.HasValue)
            {
                width = (int)Math.Round(Image.Width * Factor.Value);
                height = (int)Math.Round(Image.Height * Factor.Value);
            }
            else if (WidthFactor.HasValue || HeightFactor.HasValue)
            {
                if (WidthFactor.HasValue)
                {
                    width = (int)Math.Round(Image.Width * WidthFactor.Value);
                }
                if (HeightFactor.HasValue)
                {
                    height = (int)Math.Round(Image.Height * HeightFactor.Value);
                }
            }
            Size size = new Size(width, height);
            if (ShouldProcess(Image.ToString(), "Resize Image"))
            {
                Image.Mutate(im => im.Resize(size));
                WriteObject(Image);
            }
        }
    }
}
