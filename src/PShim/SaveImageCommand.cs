using System;
using System.Collections.Generic;
using System.Management.Automation;
using SixLabors.ImageSharp;

namespace PShim
{
    [Cmdlet("Save", "Image")]
    public class SaveImageCommand : PathCmdlet
    {

        [Parameter(ValueFromPipeline = true)]
        public Image Image { get; set; }

        protected override void ProcessRecord()
        {
            foreach (string path in Path)
            {
                ProviderInfo provider;
                List<string> filePaths = new List<string>();
                if (ShouldExpandWildcards)
                {
                    filePaths.AddRange(GetResolvedProviderPathFromPSPath(path, out provider));
                }
                else
                {
                    filePaths.Add(SessionState.Path.GetUnresolvedProviderPathFromPSPath(
                        path, out provider, out PSDriveInfo drive));
                }
                if (!IsFileSystemPath(provider, path))
                {
                    continue;
                }
                foreach (string filePath in filePaths)
                {
                    if (ShouldProcess(filePath, "Save Image"))
                    {
                        Image.Save(filePath);
                    }
                }
            }
        }
    }
}
