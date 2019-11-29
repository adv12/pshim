using System.Management.Automation;
using SixLabors.ImageSharp.Processing;

namespace PShim
{
    [Cmdlet("Sepia", "Image")]
    public class SepiaImageCommand : RectangleCmdlet
    {

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public float Amount { get; set; } = 1;

        protected override void ProcessRecord()
        {
            if (!ShouldProcess(FileImage.FileInfo.ToString(), "Sepia Image"))
            {
                return;
            }
            FileImage.Image.Mutate(im => im.Sepia(Amount, Rectangle));
            WriteObject(FileImage);
        }
    }
}
