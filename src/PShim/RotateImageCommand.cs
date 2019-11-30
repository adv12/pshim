using System;
using System.Management.Automation;
using SixLabors.ImageSharp.Processing;

namespace PShim
{
    [Cmdlet("Rotate", "Image", SupportsShouldProcess = true)]
    public class RotateImageCommand : FileImageCmdlet
    {

        [Parameter(Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromRemainingArguments = true)
        ]
        [ValidateCount(1, 1)]
        public float[] Value { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public RotationDirection Direction { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public AngleUnit Unit { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        [Alias("Color", "BackgroundColor")]
        public string Background { get; set; }

        protected override void ProcessRecord()
        {
            if (!ShouldProcess(FileImage.FileInfo.ToString(), "Rotate Image"))
            {
                return;
            }
            float degrees = Value[0];
            if (Unit == AngleUnit.Radian)
            {
                degrees = (float)(Value[0] * 180 / Math.PI);
            }
            if (Direction == RotationDirection.CCW)
            {
                degrees = -degrees;
            }
            FileImage.Image.Mutate(im => im.Rotate(degrees)
                .BackgroundColor(this.ParseColor(Background)));
            WriteObject(FileImage);
        }
    }
}
