using System.Management.Automation;
using SixLabors.ImageSharp.Processing;

namespace PShim
{
    [Cmdlet("Pixelate", "Image", SupportsShouldProcess = true)]
    public class PixelateImageCommand : RectangleCmdlet
    {

        [Parameter(ValueFromPipelineByPropertyName = true,
            ValueFromRemainingArguments = true)]
        [ValidateCount(1, 1)]
        public int[] Size { get; set; } = { 2 };

        protected override void ProcessRecord()
        {
            if (!ShouldProcess(FileImage.FileInfo.ToString(), "Pixelate Image"))
            {
                return;
            }
            FileImage.Image.Mutate(im => im.Pixelate(Size[0], Rectangle));
            WriteObject(FileImage);
        }
    }
}
