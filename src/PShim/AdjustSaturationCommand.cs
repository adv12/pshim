using System.Management.Automation;
using SixLabors.ImageSharp.Processing;

namespace PShim
{
    [Cmdlet("Adjust", "Saturation", SupportsShouldProcess = true)]
    [Alias("Saturate-Image", "Saturation-Image")]
    public class AdjustSaturationCommand : RectangleAmountCmdlet
    {
        protected override void ProcessRecord()
        {
            if (!ShouldProcess(FileImage.FileInfo.ToString(), "Adjust Saturation"))
            {
                return;
            }
            FileImage.Image.Mutate(im => im.Saturate(SingleAmount, Rectangle));
            WriteObject(FileImage);
        }
    }
}
