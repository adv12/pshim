using System.Management.Automation;
using SixLabors.ImageSharp.Processing;

namespace PShim
{
    [Cmdlet("Saturate", "Image")]
    public class SaturateImageCommand : RectangleCmdlet
    {

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public float Amount { get; set; } = 1;

        protected override void ProcessRecord()
        {
            if (!ShouldProcess(FileImage.FileInfo.ToString(), "Saturate Image"))
            {
                return;
            }
            FileImage.Image.Mutate(im => im.Saturate(Amount, Rectangle));
            WriteObject(FileImage);
        }
    }
}
