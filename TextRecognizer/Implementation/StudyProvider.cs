using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TextRecognizer.Interfaces;

namespace TextRecognizer.Implementation
{
    class StudyProvider
    {
        private readonly IImageProcessor imageProcessor;
        private readonly IImageNormalizer imageNormalizer;

        public StudyProvider(Config config, IImageProcessor imageProcessor, IImageNormalizer imageNormalizer, string path)
        {
            this.config = config;
            this.imageProcessor = imageProcessor;
            this.imageNormalizer = imageNormalizer;
            Dictionary<string, ExapmleModel> examplesDictionary;
            using (var reader = new StreamReader(path))
            {
                examplesDictionary = JsonConvert.DeserializeObject<Dictionary<string, ExapmleModel>>(reader.ReadToEnd());
            }

            StudyContent = GetStudyContent(examplesDictionary, examplesDictionary.Sum(t => t.Value.Text.Length));
            Letters = new Dictionary<int, char>();

            var index = 0;

            foreach (var c in examplesDictionary.Select(t => t.Value).SelectMany(t => t.Text))
            {
                Letters.Add(index, c);
                index++;
            }

        }

        private readonly List<char> lettersList = new List<char>();
        private readonly Config config;

        public (double[], double[])[] StudyContent { get; }

        public IDictionary<int, char> Letters { get; }

        private (double[], double[])[] GetStudyContent(Dictionary<string, ExapmleModel> examplesDictionary, int outputCount)
        {

            var result = new List<(double[], double[])>();

            int index = 0;
            foreach (var keyValuePair in examplesDictionary)
            {
                LoadExamples(result, keyValuePair.Value, outputCount, ref index);
            };
            result.Add((new double[config.LetterHeight * config.LetterWidth], new double[outputCount]));
            result.Add((new double[config.LetterHeight * config.LetterWidth].Select(t => (double) 1).ToArray(), new double[outputCount]));

            return result.ToArray();
        }

        private void LoadExamples(List<(double[], double[])> list, ExapmleModel languegeModel, int neuronsCount, ref int currentIndex)
        {
            foreach (var imgPath in languegeModel.Images)
            {
                double[][] letters = GetLetters(imgPath);

                for (int charIndex = 0; charIndex < languegeModel.Text.Length; charIndex++)
                {
                    var expected = new double[neuronsCount];
                    expected[charIndex + currentIndex] = 1;
                    list.Add((letters[charIndex], expected));
                    lettersList.Add(languegeModel.Text[charIndex]);
                }
            }

            currentIndex = languegeModel.Text.Length;
        }

        private double[][] GetLetters(string imgPath)
        {
            var image = Image.FromFile(imgPath);

            var all = imageProcessor
                .GetLetters(new Bitmap(image))
                .SelectMany(t => t.SelectMany(b => b));

            var result = new List<double[]>();

            foreach (var bitmap in all)
            {
                result.Add(imageNormalizer.Normilize(bitmap));
            }

            return result.ToArray();
        }
    }
}
