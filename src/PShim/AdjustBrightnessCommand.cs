using System.Management.Automation;
using SixLabors.ImageSharp.Processing;

namespace PShim
{
    [Cmdlet("Adjust", "Brightness", SupportsShouldProcess = true)]
    public class AdjustBrightnessCommand : RectangleAmountCmdlet
    {
        protected override void ProcessRecord()
        {
            if (!ShouldProcess(FileImage.FileInfo.ToString(), "Adjust Brightness"))
            {
                return;
            }
            FileImage.Image.Mutate(im => im.Brightness(SingleAmount, Rectangle));
            WriteObject(FileImage);
        }
    }
}
