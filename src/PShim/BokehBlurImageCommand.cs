using System.Management.Automation;
using SixLabors.ImageSharp.Processing;

namespace PShim
{
    [Cmdlet("BokehBlur", "Image")]
    public class BokehBlurImageCommand : RectangleCmdlet
    {

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public int Radius { get; set; } = 32;

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
            FileImage.Image.Mutate(im => im.BokehBlur(Radius, Components, Gamma, Rectangle));
            WriteObject(FileImage);
        }
    }
}
