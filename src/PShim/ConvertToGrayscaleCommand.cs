using System.Management.Automation;
using SixLabors.ImageSharp.Processing;

namespace PShim
{
    [Cmdlet("ConvertTo", "Grayscale", SupportsShouldProcess = true)]
    [Alias("Grayscale-Image")]
    public class ConvertToGrayscaleCommand : RectangleAmountCmdlet
    {

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public GrayscaleMode Mode { get; set; } = GrayscaleMode.Bt709;

        protected override void ProcessRecord()
        {
            if (!ShouldProcess(FileImage.FileInfo.ToString(), "Convert to Grayscale"))
            {
                return;
            }
            FileImage.Image.Mutate(im => im.Grayscale(Mode, SingleAmount, Rectangle));
            WriteObject(FileImage);
        }
    }
}
