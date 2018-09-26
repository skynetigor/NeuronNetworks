using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using TextRecognizer.Extensions;
using TextRecognizer.Interfaces;

namespace TextRecognizer.Implementation.ImageProcessor
{
    static class ImageProcessorTest
    {
        public static ImageProcessorImpl ImageProcessor { get; }
        public static IImageFilter Median { get; }
        public static IImageFilter Monochrom { get; }

        static ImageProcessorTest()
        {
            var serviceProvider = ConfigureServices(new ServiceCollection()).BuildServiceProvider();

            ImageProcessor = serviceProvider.GetService<ImageProcessorImpl>();
            var filters = serviceProvider.GetService<IEnumerable<IImageFilter>>();
        }

        private static IServiceCollection ConfigureServices(IServiceCollection serviceDescriptors)
        {
            return serviceDescriptors
                .AddImageFilters()
                .AddSingleton(new Config { LetterHeight = 30, LetterWidth = 23, MonochromLevel = 50 })
                .AddSingleton<ImageProcessorImpl>()
                .AddSingleton<IImageScaler, ImageScaler>();
        }
    }
}
