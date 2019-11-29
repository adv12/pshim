using System.Management.Automation;
using SixLabors.Primitives;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace PShim
{
    [Cmdlet("BoxBlur", "Image")]
    public class BoxBlurImageCommand : RectangleCmdlet
    {

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public int Radius { get; set; } = 7;

        protected override void ProcessRecord()
        {
            if (!ShouldProcess(FileImage.FileInfo.ToString(), "Box Blur Image"))
            {
                return;
            }
            FileImage.Image.Mutate(im => im.BoxBlur(Radius, Rectangle));
            WriteObject(FileImage);
        }
    }
}
