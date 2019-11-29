using System.Management.Automation;

namespace PShim
{
    public class FileImageCmdlet: PSCmdlet
    {
        [Parameter(ValueFromPipeline = true, Mandatory = true)]
        public FileImage FileImage { get; set; }
    }
}
