using System.Management.Automation;
using SixLabors.ImageSharp.Processing;

namespace PShim
{
    [Cmdlet("Apply", "PolaroidFilter", SupportsShouldProcess = true)]
    [Alias("Polaroid-Image")]
    public class ApplyPolaroidFilterCommand : RectangleCmdlet
    {
        protected override void ProcessRecord()
        {
            if (!ShouldProcess(FileImage.FileInfo.ToString(), "Apply Polaroid Filter"))
            {
                return;
            }
            FileImage.Image.Mutate(im => im.Polaroid(Rectangle));
            WriteObject(FileImage);
        }
    }
}
