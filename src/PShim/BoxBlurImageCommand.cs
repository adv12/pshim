using System.Management.Automation;
using SixLabors.ImageSharp.Processing;

namespace PShim
{
    [Cmdlet("BoxBlur", "Image", SupportsShouldProcess = true)]
    public class BoxBlurImageCommand : RectangleCmdlet
    {

        [Parameter(ValueFromPipelineByPropertyName = true,
            ValueFromRemainingArguments = true)]
        [ValidateCount(1, 1)]
        public int[] Radius { get; set; } = { 7 };

        protected override void ProcessRecord()
        {
            if (!ShouldProcess(FileImage.FileInfo.ToString(), "Box Blur Image"))
            {
                return;
            }
            FileImage.Image.Mutate(im => im.BoxBlur(Radius[0], Rectangle));
            WriteObject(FileImage);
        }
    }
}
