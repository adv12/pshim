using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using SixLabors.ImageSharp;

namespace PShim
{
    [Cmdlet(VerbsCommon.Get, "ImageMetadata")]
    public class GetImageMetadataCommand : PathCmdlet
    {
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
                    if (ShouldProcess(filePath, "Get Image Metadata"))
                    {
                        try
                        {
                            IImageInfo info;
                            using (Stream stream = File.OpenRead(filePath))
                            {
                                info = Image.Identify(stream);
                            }
                            WriteObject(new BasicImageMetadata
                            {
                                Directory = System.IO.Path.GetDirectoryName(filePath),
                                Name = System.IO.Path.GetFileName(filePath),
                                Width = info.Width,
                                Height = info.Height,
                                BitsPerPixel = info.PixelType.BitsPerPixel
                            });
                        }
                        catch (Exception ex)
                        {
                            WriteError(new ErrorRecord(ex, "NotAnImage", ErrorCategory.InvalidArgument, filePath));
                        }
                    }
                }
            }
        }
    }
}
