using System.Management.Automation;
using SixLabors.ImageSharp.Processing;

namespace PShim
{
    [Cmdlet("Adjust", "Contrast", SupportsShouldProcess = true)]
    [Alias("Contrast-Image")]
    public class AdjustContrastCommand : RectangleAmountCmdlet
    {
        protected override void ProcessRecord()
        {
            if (!ShouldProcess(FileImage.FileInfo.ToString(), "Adjust Contrast"))
            {
                return;
            }
            FileImage.Image.Mutate(im => im.Contrast(SingleAmount, Rectangle));
            WriteObject(FileImage);
        }
    }
}
