using System.Management.Automation;
using SixLabors.ImageSharp.Processing;

namespace PShim
{
    [Cmdlet("ConvertTo", "Sepia", SupportsShouldProcess = true)]
    public class ConvertToSepiaCommand : RectangleAmountCmdlet
    {
        protected override void ProcessRecord()
        {
            if (!ShouldProcess(FileImage.FileInfo.ToString(), "Convert to Sepia"))
            {
                return;
            }
            FileImage.Image.Mutate(im => im.Sepia(SingleAmount, Rectangle));
            WriteObject(FileImage);
        }
    }
}
