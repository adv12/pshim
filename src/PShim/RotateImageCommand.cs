using System;
using System.Management.Automation;
using SixLabors.ImageSharp.Processing;

namespace PShim
{
    [Cmdlet("Rotate", "Image")]
    public class RotateImageCommand : PSCmdlet
    {

        [Parameter(ValueFromPipeline = true)]
        public FileImage FileImage { get; set; }

        [Parameter(Mandatory = true, ValueFromRemainingArguments = true)]
        public float[] Value { get; set; }

        [Parameter]
        public RotationDirection Direction { get; set; }

        [Parameter]
        public AngleUnit Unit { get; set; }

        protected override void ProcessRecord()
        {
            float degrees = Value[0];
            if (Unit == AngleUnit.Radian)
            {
                degrees = (float)(Value[0] * 180 / Math.PI);
            }
            if (Direction == RotationDirection.CCW)
            {
                degrees = -degrees;
            }
            if (ShouldProcess(FileImage.FileInfo.ToString(), "Rotate Image"))
            {
                FileImage.Image.Mutate(im => im.Rotate(degrees));
                WriteObject(FileImage);
            }
        }
    }
}
