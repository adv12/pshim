using System.Management.Automation;
using SixLabors.ImageSharp.Processing;

namespace PShim
{
    [Cmdlet("ConvertTo", "BlackWhite", SupportsShouldProcess = true)]
    public class ConvertToBlackWhiteCommand : RectangleCmdlet
    {
        protected override void ProcessRecord()
        {
            if (!ShouldProcess(FileImage.FileInfo.ToString(), "Convert to Black/White"))
            {
                return;
            }
            FileImage.Image.Mutate(im => im.BlackWhite(Rectangle));
            WriteObject(FileImage);
        }
    }
}
