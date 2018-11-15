using System.Collections.Generic;
using System.Drawing;
using TextRecognizer.Interfaces;

namespace TextRecognizer.Implementation
{
    class ImageNormalizer : IImageNormalizer
    {
        public double[] Normilize(Bitmap bitmap)
        {
            var buffer = new List<double>();

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    var b = pixel.R >= 200 && pixel.G >= 200 && pixel.B >= 200 ? 0 : 1;
                    buffer.Add(b);
                }
            }

            return buffer.ToArray();
        }
    }
}
