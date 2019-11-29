using System.Management.Automation;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Dithering;
using SixLabors.ImageSharp.Processing.Processors.Quantization;

namespace PShim
{
    [Cmdlet("Quantize", "Image")]
    public class QuantizeImageCommand : FileImageCmdlet
    {

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public int MaxColors { get; set; } = 256;

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public SwitchParameter Dither { get; set; }

        protected override void ProcessRecord()
        {
            if (!ShouldProcess(FileImage.FileInfo.ToString(), "Quantize Image"))
            {
                return;
            }
            IErrorDiffuser diffuser = Dither ? KnownDiffusers.FloydSteinberg : null;
            FileImage.Image.Mutate(im => im.Quantize(new WuQuantizer(diffuser, MaxColors)));
            WriteObject(FileImage);
        }
    }
}
