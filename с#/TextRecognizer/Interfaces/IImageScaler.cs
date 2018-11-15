using System.Drawing;

namespace TextRecognizer.Interfaces
{
    public interface IImageScaler
    {
        Bitmap Scale(Bitmap input, int width, int height);
    }
}
