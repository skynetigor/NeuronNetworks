using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using NeuronNetworks.Perseptron;
using TextRecognizer.Extensions;
using TextRecognizer.Interfaces;

namespace TextRecognizer.Implementation
{
    class TextRec
    {
        private readonly NeuronNetwork network;

        private readonly IDictionary<int, char> letters;
        private readonly IImageProcessor imageProcessor;
        private readonly IServiceProvider serviceProvider;
        private readonly IImageNormalizer imageNormalizer;
        private readonly Config config;

        public TextRec()
        {
            var serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection);

            serviceProvider = serviceCollection.BuildServiceProvider();

            this.imageProcessor = serviceProvider.GetService<IImageProcessor>();
            this.config = serviceProvider.GetService<Config>();
            this.imageNormalizer = serviceProvider.GetService<IImageNormalizer>();
            var st = serviceProvider.GetService<StudyProvider>();

            network = new NeuronNetwork(new PerseptronConfig(new[] { config.LetterHeight * config.LetterWidth, st.Letters.Count }));

            Console.WriteLine("Studying...");
            network.Study(st.StudyContent);
            Console.WriteLine("Done!");

            letters = st.Letters;
        }

        public string Recognize(Image img)
        {
            var rows = imageProcessor.GetLetters(new Bitmap(img));

            StringBuilder stringBuilder = new StringBuilder();
            var ndex = 0;
            foreach (var row in rows)
            {
                foreach (var word in row)
                {
                    foreach (var letter in word)
                    {
                        var asdfdsgssd = this.imageNormalizer.Normilize(letter);

                        var answer = network.GetAnswer(asdfdsgssd);

                        double previous = 0;
                        int index = 0;

                        for (int i = 0; i < answer.Length; i++)
                        {
                            var s = answer[i];

                            if (s > previous)
                            {
                                previous = s;
                                index = i;
                            }
                        }

                        if (previous > 0.5 && letters.TryGetValue(index, out char symbol))
                        {
                            stringBuilder.Append(symbol);
                        }
                        else
                        {
                            stringBuilder.Append('*');
                        }

                        ndex++;
                    }
                    stringBuilder.Append(" ");
                }

                stringBuilder.Append("\n");
            }

            return stringBuilder.ToString();
        }

        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddImageFilters()
                .AddConfig<Config>("config.json")
                .AddSingleton<IImageProcessor, ImageProcessorImpl>()
                .AddSingleton<IImageNormalizer, ImageNormalizer>()
                .AddTransient(sp => ActivatorUtilities.CreateInstance<StudyProvider>(sp, "study-config.json"));
        }
    }
}
