using System.Collections.Generic;
using System.Management.Automation;
using SixLabors.ImageSharp;

namespace PShim
{
    [Cmdlet("Save", "Image")]
    public class SaveImageCommand : PathCmdlet
    {

        [Parameter(ValueFromPipeline = true, Mandatory = true)]
        public FileImage FileImage { get; set; }

        protected override void ProcessRecord()
        {
            foreach (string path in Path)
            {
                ProviderInfo provider;
                List<string> filePaths = new List<string>();
                filePaths.Add(SessionState.Path.GetUnresolvedProviderPathFromPSPath(
                        path, out provider, out PSDriveInfo drive));
                if (!IsFileSystemPath(provider, path))
                {
                    continue;
                }
                foreach (string filePath in filePaths)
                {
                    if (ShouldProcess(filePath, "Save Image"))
                    {
                        FileImage.Image.Save(filePath);
                    }
                }
            }
        }
    }
}
