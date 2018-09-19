using System.Drawing;
using TextRecognizer.Interfaces;

namespace TextRecognizer.Implementation.ImageFilters
{
    [Order(1)]
    class MonochromImageFilter : IImageFilter
    {
        private readonly Config config;

        public MonochromImageFilter(Config config)
        {
            this.config = config;
        }

        public Bitmap Handle(Bitmap bitmap)
        {
            return Monochrome(bitmap, config.MonochromLevel);
        }

        private Bitmap Monochrome(Bitmap image, int level)
        {
            var result = new Bitmap(image.Width, image.Height);

            for (int j = 0; j < image.Height; j++)
            {
                for (int i = 0; i < image.Width; i++)
                {
                    var color = image.GetPixel(i, j);
                    int sr = (color.R + color.G + color.B) / 3;
                    result.SetPixel(i, j, (sr < level ? Color.Black : Color.White));
                }
            }
            return result;
        }
    }
}
