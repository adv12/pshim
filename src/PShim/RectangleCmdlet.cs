using System.Management.Automation;
using SixLabors.Primitives;
using SixLabors.ImageSharp;

namespace PShim
{
    public class RectangleCmdlet: FileImageCmdlet
    {

        [Parameter(ValueFromPipelineByPropertyName = true)
        ]
        public int? Left { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)
        ]
        public int? Top { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)
        ]
        public int? Width { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)
        ]
        public int? Height { get; set; }

        protected Rectangle Rectangle
        {
            get
            {
                Image image = FileImage.Image;
                int left = Left ?? 0;
                int top = Top ?? 0;
                int width = Width ?? image.Width - left;
                int height = Height ?? image.Height - top;
                return new Rectangle(left, top, width, height);
            }
        }

        protected RectangleF RectangleF
        {
            get
            {
                Rectangle rect = Rectangle;
                return new RectangleF(rect.Left, rect.Top, rect.Width, rect.Height);
            }
        }
    }
}
