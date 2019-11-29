﻿using System.Management.Automation;
using SixLabors.ImageSharp.Processing;

namespace PShim
{
    [Cmdlet("FillRectangleOn", "Image", SupportsShouldProcess = true)]
    public class FillRectangleOnImageCommand : RectangleCmdlet
    {

        [Parameter(ValueFromPipeline = true,
            ValueFromRemainingArguments = true)]
        [ValidateCount(1, 1)]
        public string[] Color { get; set; } = { "Black" };

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public string Background { get; set; } = "Transparent";

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public Brush Brush { get; set; } = Brush.Solid;

        protected override void ProcessRecord()
        {
            if (!ShouldProcess(FileImage.FileInfo.ToString(), "Fill Image"))
            {
                return;
            }
            IBrush brush = PShimUtil.GetBrush(Brush, Color[0], Background);
            FileImage.Image.Mutate(im => im.Fill(brush, RectangleF));
            WriteObject(FileImage);
        }
    }
}
