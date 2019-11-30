using System.Management.Automation;
using SixLabors.ImageSharp.Processing;

namespace PShim
{
    [Cmdlet("BokehBlur", "Image", SupportsShouldProcess = true)]
    [Alias("Apply-BokehBlur")]
    public class BokehBlurImageCommand : RectangleCmdlet
    {

        [Parameter(ValueFromPipelineByPropertyName = true,
            ValueFromRemainingArguments = true)]
        [ValidateCount(1, 1)]
        public int[] Radius { get; set; } = { 32 };

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public int Components { get; set; } = 2;

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public float Gamma { get; set; } = 3f;

        protected override void ProcessRecord()
        {
            if (!ShouldProcess(FileImage.FileInfo.ToString(), "Bokeh Blur Image"))
            {
                return;
            }
            FileImage.Image.Mutate(im => im.BokehBlur(Radius[0], Components, Gamma, Rectangle));
            WriteObject(FileImage);
        }
    }
}
