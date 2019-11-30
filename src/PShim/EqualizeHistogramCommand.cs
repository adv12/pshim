using System.Management.Automation;
using SixLabors.ImageSharp.Processing;

namespace PShim
{
    [Cmdlet("Equalize", "Histogram", SupportsShouldProcess = true)]
    [Alias("HistogramEqualization-Image")]
    public class EqualizeHistogramCommand : FileImageCmdlet
    {
        protected override void ProcessRecord()
        {
            if (!ShouldProcess(FileImage.FileInfo.ToString(), "Equalize Histogram"))
            {
                return;
            }
            FileImage.Image.Mutate(im => im.HistogramEqualization());
            WriteObject(FileImage);
        }
    }
}
