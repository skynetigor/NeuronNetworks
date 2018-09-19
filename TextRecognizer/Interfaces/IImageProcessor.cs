using System.Drawing;

namespace TextRecognizer.Interfaces
{
    public interface IImageProcessor
    {
        Bitmap[][][] GetLetters(Bitmap input);
    }
}