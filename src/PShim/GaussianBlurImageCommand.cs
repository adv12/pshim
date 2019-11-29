using System.Management.Automation;
using SixLabors.ImageSharp.Processing;

namespace PShim
{
    [Cmdlet("GaussianBlur", "Image", SupportsShouldProcess = true)]
    public class GaussianBlurImageCommand : RectangleCmdlet
    {

        [Parameter(ValueFromPipelineByPropertyName = true,
            ValueFromRemainingArguments = true)]
        [ValidateCount(1, 1)]
        public float[] Sigma { get; set; } = { 3f };

        protected override void ProcessRecord()
        {
            if (!ShouldProcess(FileImage.FileInfo.ToString(), "Gaussian Blur Image"))
            {
                return;
            }
            FileImage.Image.Mutate(im => im.GaussianBlur(Sigma[0], Rectangle));
            WriteObject(FileImage);
        }
    }
}
