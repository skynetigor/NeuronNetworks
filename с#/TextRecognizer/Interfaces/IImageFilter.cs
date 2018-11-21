using System.Drawing;

namespace TextRecognizer.Interfaces
{
    public interface IImageFilter
    {
        Bitmap Handle(Bitmap bitmap);
    }
}
