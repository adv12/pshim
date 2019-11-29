using System.Management.Automation;
using SixLabors.ImageSharp.Processing;

namespace PShim
{
    [Cmdlet("Grayscale", "Image")]
    public class GrayscaleImageCommand : RectangleCmdlet
    {

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public GrayscaleMode Mode { get; set; } = GrayscaleMode.Bt709;

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public float Amount { get; set; } = 1;

        protected override void ProcessRecord()
        {
            if (!ShouldProcess(FileImage.FileInfo.ToString(), "Grayscale Image"))
            {
                return;
            }
            FileImage.Image.Mutate(im => im.Grayscale(Mode, Amount, Rectangle));
            WriteObject(FileImage);
        }
    }
}
