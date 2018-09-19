using System;
using System.Diagnostics;
using System.Drawing;
using TextRecognizer.Implementation;

namespace TextRecognizer
{
    class Program
    {

        static void Main(string[] args)
        {
            //var imgProc = new ImageProcessorImpl(new Config
            //{
            //    LetterHeight = 35,
            //    LetterWidth = 25,
            //    MonochromLevel = 63
            //});
            //////var image = new Bitmap(Image.FromFile(@"E:\Projects\NeuronNetwork\TextRecognizer\bin\Debug\Examples\Ammy.jpg"));
            //////for (int ii = 0; ii < 100; ii++)
            //////{
            //////    imgProc.Monochrome(imgProc.Median(image), ii).Save($"temp/{ii}.jpg");
            //////}


            //var letters = imgProc.GetLetters(new Bitmap(Image.FromFile(@"E:\Projects\NeuronNetwork\TextRecognizer\Examples\Ammy.jpg")));

            //var i = 0;

            //foreach (var row in letters)
            //{
            //    foreach (var word in row)
            //    {
            //        foreach (var letter in word)
            //        {
            //            letter.Save($"temp/{i}.jpg");
            //            i++;
            //        }
            //    }
            //}
            //Console.WriteLine("dadas");
            //Console.ReadLine();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var txtrec = new TextRec();
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed.Milliseconds);
            var da = txtrec.Recognize(Image.FromFile(@"E:\Projects\NeuronNetwork\TextRecognizer\Examples\Ammy.jpg"));
            Console.WriteLine(da);
            while (true)
            {
                Console.Write("Path: ");
                var path = Console.ReadLine();

                try
                {
                    Console.WriteLine(txtrec.Recognize(Image.FromFile(path)));
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private static void CheckAnswer(string path)
        {
            //var answer = network.GetAnswer(GetImage(path));

            //Console.WriteLine(JsonConvert.SerializeObject(answer.Select(t => t > 0.5 ? true : false)));
        }


    }
}
