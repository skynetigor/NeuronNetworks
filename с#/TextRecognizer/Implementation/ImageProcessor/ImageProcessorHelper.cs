using System.Drawing;

namespace TextRecognizer.Implementation.ImageProcessor
{
    public static class ImageProcessorHelper
    {
        public static byte GetMonochromPixel(this Bitmap bitmap, int x, int y)
        {
            var pixel = bitmap.GetPixel(x, y);
            var b = pixel.R >= 200 && pixel.G >= 200 && pixel.B >= 200 ? 0 : 1;

            return (byte)b;
        }

        public static void SetMonochromPixel(this Bitmap bitmap, int x, int y, int value)
        {
            var color = value >= 1 ? Color.Black : Color.White;

            bitmap.SetPixel(x, y, color);
        }

        public static Bitmap ToBitmapFromMonochrom(this byte[,] input)
        {
            var result = new Bitmap(input.GetLength(0), input.GetLength(1));
            for (int y = 0; y < input.GetLength(1); y++)
            {
                for (int x = 0; x < input.GetLength(0); x++)
                {
                    result.SetMonochromPixel(x, y, input[x, y]);
                }
            }

            return result;
        }
    }
}
