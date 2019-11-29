﻿using System.Management.Automation;
using SixLabors.ImageSharp.Processing;

namespace PShim
{
    [Cmdlet("Pixelate", "Image")]
    public class PixelateImageCommand : RectangleCmdlet
    {

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public int Size { get; set; } = 2;

        protected override void ProcessRecord()
        {
            if (!ShouldProcess(FileImage.FileInfo.ToString(), "Pixelate Image"))
            {
                return;
            }
            FileImage.Image.Mutate(im => im.Pixelate(Size, Rectangle));
            WriteObject(FileImage);
        }
    }
}
