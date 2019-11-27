﻿using System;
using System.Collections.Generic;
using System.Management.Automation;
using SixLabors.ImageSharp;

namespace PShim
{
    [Cmdlet("Load", "Image")]
    public class LoadImageCommand : PathCmdlet
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
                    if (ShouldProcess(filePath, "Load Image"))
                    {
                        using (Image image = Image.Load(filePath))
                        {
                            WriteObject(image);
                        }
                    }
                }
            }
        }
    }
}
