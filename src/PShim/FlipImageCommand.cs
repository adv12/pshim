using System.Management.Automation;
using SixLabors.ImageSharp.Processing;

namespace PShim
{
    [Cmdlet("Flip", "Image", SupportsShouldProcess = true)]
    public class FlipImageCommand : FileImageCmdlet
    {

        [Parameter(ValueFromPipelineByPropertyName = true,
            ValueFromRemainingArguments = true)]
        [ValidateCount(1, 1)]
        public FlipAxis[] Axis { get; set; } = { FlipAxis.Horizontal };

        protected override void ProcessRecord()
        {
            if (!ShouldProcess(FileImage.FileInfo.ToString(), "Flip Image"))
            {
                return;
            }
            FlipAxis axis = Axis[0];
            if (axis == FlipAxis.Horizontal || axis == FlipAxis.Both)
            {
                FileImage.Image.Mutate(im => im.Flip(FlipMode.Horizontal));
            }
            if (axis == FlipAxis.Vertical || axis == FlipAxis.Both)
            {
                FileImage.Image.Mutate(im => im.Flip(FlipMode.Vertical));
            }
            WriteObject(FileImage);
        }
    }
}
