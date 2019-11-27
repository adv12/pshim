using System.IO;
using SixLabors.ImageSharp;

namespace PShim
{
    public class FileImage
    {
        public FileInfo FileInfo { get; set; }

        public Image Image { get; set; }

        public FileImage(string literalPath, Image image)
        {
            FileInfo = new FileInfo(literalPath);
            Image = image;
        }
    }
}
