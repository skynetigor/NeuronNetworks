using System;
using System.Diagnostics;
using System.Drawing;
using TextRecognizer.Implementation;
using TextRecognizer.Implementation.ImageProcessor;

namespace TextRecognizer
{
    class Program
    {
        static void Main(string[] args)
        {
            //var imgProc = ImageProcessorTest.ImageProcessor;
            //var image = new Bitmap(Image.FromFile(@"E:\Projects\NeuronNetwork\TextRecognizer\bin\Debug\Examples\AMMMY.jpg"));
            //var imgProc = ImageProcessorTest.ImageProcessor;
            //var image = new Bitmap(Image.FromFile(@"E:\Projects\NeuronNetwork\TextRecognizer\bin\Debug\Examples\WINWORD_2018-09-20_20-29-48.jpg"));
            //var i = 0;


            //var b = imgProc.GetLetters(image);
            //image = imgProc.ThroughFilters(image);
            //var rows = imgProc.GetRows(image);

            //var words = imgProc.GetWords(rows[0]);

            //foreach (var letter in b[0][1])
            //{
            //    letter.Save($"temp/{i}.jpg");
            //    i++;
            //}
            //var i = 0;
            //var letters = imgProc.GetLetters(image);

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
            Console.WriteLine($"Seconds { stopwatch.Elapsed.TotalSeconds }, Minutes { stopwatch.Elapsed.TotalMinutes }");
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
