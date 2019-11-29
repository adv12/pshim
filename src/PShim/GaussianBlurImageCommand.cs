using System.Management.Automation;
using SixLabors.ImageSharp.Processing;

namespace PShim
{
    [Cmdlet("GaussianBlur", "Image")]
    public class GaussianBlurImageCommand : RectangleCmdlet
    {

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public float Sigma { get; set; } = 3f;

        protected override void ProcessRecord()
        {
            if (!ShouldProcess(FileImage.FileInfo.ToString(), "Gaussian Blur Image"))
            {
                return;
            }
            FileImage.Image.Mutate(im => im.GaussianBlur(Sigma, Rectangle));
            WriteObject(FileImage);
        }
    }
}
