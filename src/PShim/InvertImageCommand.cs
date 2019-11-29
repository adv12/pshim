using System.Management.Automation;
using SixLabors.ImageSharp.Processing;

namespace PShim
{
    [Cmdlet("Invert", "Image")]
    public class InvertImageCommand : RectangleCmdlet
    {
        protected override void ProcessRecord()
        {
            if (!ShouldProcess(FileImage.FileInfo.ToString(), "Invert Image"))
            {
                return;
            }
            FileImage.Image.Mutate(im => im.Invert(Rectangle));
            WriteObject(FileImage);
        }
    }
}
