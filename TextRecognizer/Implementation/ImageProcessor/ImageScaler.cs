using System.Drawing;
using TextRecognizer.Interfaces;

namespace TextRecognizer.Implementation.ImageProcessor
{
    class ImageScaler : IImageScaler
    {
        public Bitmap Scale(Bitmap bitmap, int width, int height)
        {
            if (bitmap == null)
            {
                return null;
            }

            var result = new Bitmap(width, height);

            using (var gfx = Graphics.FromImage(result))
            {
                gfx.DrawImage(bitmap, new Rectangle(Point.Empty, new Size(width, height)));
            }

            return result;
        }
    }
}
