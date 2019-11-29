using System.Management.Automation;
using SixLabors.ImageSharp.Processing;

namespace PShim
{
    [Cmdlet("Polaroid", "Image")]
    public class PolaroidImageCommand : RectangleCmdlet
    {
        protected override void ProcessRecord()
        {
            if (!ShouldProcess(FileImage.FileInfo.ToString(), "Polaroid Image"))
            {
                return;
            }
            FileImage.Image.Mutate(im => im.Polaroid(Rectangle));
            WriteObject(FileImage);
        }
    }
}
