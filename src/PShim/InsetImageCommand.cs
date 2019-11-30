using System.Collections.Generic;
using System.Management.Automation;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using SixLabors.Shapes;

namespace PShim
{
    [Cmdlet("Inset", "Image", SupportsShouldProcess = true)]
    public class InsetImageCommand : FileImageCmdlet
    {

        [Parameter(ValueFromPipelineByPropertyName = true,
            ValueFromRemainingArguments = true)]
        [ValidateCount(1, 1)]
        [ValidateRange(ValidateRangeKind.NonNegative)]
        public int[] All { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        [ValidateRange(ValidateRangeKind.NonNegative)]
        public int? LeftRight { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        [ValidateRange(ValidateRangeKind.NonNegative)]
        public int? TopBottom { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        [ValidateRange(ValidateRangeKind.NonNegative)]
        public int? Left { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        [ValidateRange(ValidateRangeKind.NonNegative)]
        public int? Right { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        [ValidateRange(ValidateRangeKind.NonNegative)]
        public int? Top { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        [ValidateRange(ValidateRangeKind.NonNegative)]
        public int? Bottom { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public string Color { get; set; } = "Black";

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public string Background { get; set; } = "Transparent";

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public Brush Brush { get; set; } = Brush.Solid;

        protected override void ProcessRecord()
        {
            if (!ShouldProcess(FileImage.FileInfo.ToString(), "Inset Image"))
            {
                return;
            }
            Image image = FileImage.Image;
            int insetLeft = 0;
            int insetRight = 0;
            int insetTop = 0;
            int insetBottom = 0;
            if (All != null && All.Length > 0)
            {
                insetLeft = insetRight = insetTop = insetBottom = All[0];
            }
            if (LeftRight.HasValue)
            {
                insetLeft = insetRight = LeftRight.Value;
            }
            insetLeft = Left ?? insetLeft;
            insetRight = Right ?? insetRight;
            if (TopBottom.HasValue)
            {
                insetTop = insetBottom = TopBottom.Value;
            }
            insetTop = Top ?? insetTop;
            insetBottom = Bottom ?? insetBottom;
            int width = image.Width + insetLeft + insetRight;
            int height = image.Height + insetTop + insetBottom;
            Rgba32 insetColor = this.ParseColor(Color);
            PathBuilder pathBuilder = new PathBuilder();
            List<PointF> outerPoints = new List<PointF>();
            outerPoints.Add(new PointF(0, 0));
            outerPoints.Add(new PointF(image.Width, 0));
            outerPoints.Add(new PointF(image.Width, image.Height));
            outerPoints.Add(new PointF(0, image.Height));
            outerPoints.Add(new PointF(0, 0));
            List<PointF> innerPoints = new List<PointF>();
            innerPoints.Add(new PointF(insetLeft, insetTop));
            innerPoints.Add(new PointF(image.Width - insetRight, insetTop));
            innerPoints.Add(new PointF(image.Width - insetRight, image.Height - insetBottom));
            innerPoints.Add(new PointF(insetLeft, image.Height - insetBottom));
            innerPoints.Add(new PointF(insetLeft, insetTop));
            pathBuilder.StartFigure();
            pathBuilder.AddLines(outerPoints);
            pathBuilder.StartFigure();
            pathBuilder.AddLines(innerPoints);
            pathBuilder.CloseAllFigures();
            IBrush brush = this.GetBrush(Brush, Color, Background);
            FileImage.Image.Mutate(im => im.Fill(brush, pathBuilder.Build()));
            WriteObject(FileImage);
        }
    }
}
