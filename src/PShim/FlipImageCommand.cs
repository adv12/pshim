using System.Management.Automation;
using SixLabors.ImageSharp.Processing;

namespace PShim
{
    [Cmdlet("Flip", "Image")]
    public class FlipImageCommand : PSCmdlet
    {
        [Parameter(ValueFromPipeline = true)]
        public FileImage FileImage { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public FlipAxis Axis { get; set; }

        protected override void ProcessRecord()
        {
            if (!ShouldProcess(FileImage.FileInfo.ToString(), "Flip Image"))
            {
                return;
            }
            if (Axis == FlipAxis.Horizontal || Axis == FlipAxis.Both)
            {
                FileImage.Image.Mutate(im => im.Flip(FlipMode.Horizontal));
            }
            if (Axis == FlipAxis.Vertical || Axis == FlipAxis.Both)
            {
                FileImage.Image.Mutate(im => im.Flip(FlipMode.Vertical));
            }
            WriteObject(FileImage);
        }
    }
}
