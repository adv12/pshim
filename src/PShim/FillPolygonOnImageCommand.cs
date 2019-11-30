using System.Collections.Generic;
using System.Management.Automation;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using SixLabors.Shapes;

namespace PShim
{
    [Cmdlet("FillPolygonOn", "Image", SupportsShouldProcess = true)]
    public class FillPolygonOnImageCommand : FileImageCmdlet
    {

        [Parameter(ValueFromPipelineByPropertyName = true,
            ValueFromRemainingArguments = true)]
        [ValidateCount(1, 1)]
        public string[] Color { get; set; } = { "Black" };

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public string Background { get; set; } = "Transparent";

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public Brush Brush { get; set; } = Brush.Solid;

        [Parameter(ValueFromPipelineByPropertyName = true, Mandatory = true)]
        [ValidateCount(6, int.MaxValue)]
        public float[] Coordinates;

        protected override void ProcessRecord()
        {
            if (!ShouldProcess(FileImage.FileInfo.ToString(), "Fill Polygon on Image"))
            {
                return;
            }
            PathBuilder pathBuilder = new PathBuilder();
            List<PointF> points = new List<PointF>();
            for (int i = 0; i < Coordinates.Length; i += 2)
            {
                if (i + 1 < Coordinates.Length)
                {
                    points.Add(new PointF(Coordinates[i], Coordinates[i + 1]));
                }
            }
            pathBuilder.StartFigure();
            pathBuilder.AddLines(points);
            pathBuilder.CloseAllFigures();
            IBrush brush = this.GetBrush(Brush, Color[0], Background);
            FileImage.Image.Mutate(im => im.Fill(brush, pathBuilder.Build()));
            WriteObject(FileImage);
        }
    }
}
