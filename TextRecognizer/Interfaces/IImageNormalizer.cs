using System.Drawing;

namespace TextRecognizer.Interfaces
{
    interface IImageNormalizer
    {
        double[] Normilize(Bitmap bitmap);
    }
}
