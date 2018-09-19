using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TextRecognizer.Implementation.ImageProcessor;
using TextRecognizer.Interfaces;

namespace TextRecognizer.Implementation
{
    public class ImageProcessorImpl : IImageProcessor
    {
        private readonly Config config;
        private readonly IImageFilter[] imageFilters;

        public ImageProcessorImpl(Config config, IEnumerable<IImageFilter> imageFilters)
        {
            this.config = config;
            this.imageFilters = imageFilters.ToArray();
        }

        public Bitmap[][][] GetLetters(Bitmap input)
        {
            foreach(var imageFilter in imageFilters)
            {
                input = imageFilter.Handle(input);
            }

            var result = SeparateWhiteSpaceY(input).Where(t => t.Height > 1).ToArray();

            if (result.Length == 0)
            {
                result = new Bitmap[] { input };
            }

            return result.Select(t =>
            {
                var c = GetWords(t);
                if (!c.Any())
                {
                    return new Bitmap[] { t };
                }

                return c.ToArray();
            }).Select(t =>
            {
                return t.Select(c =>
                {
                    var bcc = SeparateWhiteSpaceY(c);

                    if (!bcc.Any())
                    {
                        bcc = new Bitmap[] { c };
                    }

                    return bcc.Select(x => Scale(Cut(x), config.LetterWidth, config.LetterHeight)).Where(bitmap => bitmap != null).ToArray();
                }).ToArray();
            }).ToArray();
        }

        private Bitmap Cut(Bitmap input)
        {
            var isObjectFound = false;
            List<List<byte>> currentRow = new List<List<byte>>();

            for (int x = 0; x < input.Width; x++)
            {
                var sum = 0;

                var c = new List<byte>();

                for (int y = 0; y < input.Height; y++)
                {
                    if (isObjectFound == false)
                        sum += input.GetMonochromPixel(x, y);
                    else
                        c.Add(input.GetMonochromPixel(x, y));
                }

                if (!isObjectFound && sum > 0)
                {
                    isObjectFound = true;
                }
                else if (isObjectFound)
                {
                    currentRow.Add(c);
                }
            }

            var bcc = Help(currentRow);
            currentRow = new List<List<byte>>();

            for (int x = bcc.GetLength(0) - 1; x >= 0; x--)
            {
                var sum = 0;

                var c = new List<byte>();

                for (int y = 0; y < bcc.GetLength(1); y++)
                {
                    if (isObjectFound == false)
                        sum += bcc[x, y];
                    else
                        c.Add(bcc[x, y]);
                }

                if (!isObjectFound && sum > 0)
                {
                    isObjectFound = true;
                }
                else if (isObjectFound)
                {
                    currentRow.Add(c);
                }
            }

            bcc = Help(currentRow);

            try
            {
                return bcc.ToBitmapFromMonochrom();
            }
            catch
            {
                return null;
            }
        }

        private Bitmap[] SeparateWhiteSpaceY(Bitmap input, int b = 0)
        {
            List<byte[,]> result = new List<byte[,]>();

            var isRow = false;
            List<List<byte>> currentRow = null;

            for (int y = 0; y < input.Height; y++)
            {
                var sum = 0;

                var c = new List<byte>();

                for (int x = 0; x < input.Width; x++)
                {
                    var pixel = input.GetMonochromPixel(x, y);

                    sum += pixel;
                    c.Add(pixel);
                }

                if (sum > b)
                {
                    if (currentRow == null)
                    {
                        currentRow = new List<List<byte>>();
                        isRow = true;
                    }

                    currentRow.Add(c);
                }
                else if (isRow && sum == 0)
                {
                    if (currentRow != null)
                    {
                        result.Add(Help(currentRow));
                        currentRow = null;
                    }
                }
            }

            return result.Select(t => t.ToBitmapFromMonochrom()).ToArray();
        }

        private Bitmap[] SeparateWhiteSpaceX(Bitmap input, int b = 0)
        {
            List<byte[,]> result = new List<byte[,]>();

            var isRow = false;
            List<List<byte>> currentRow = null;

            for (int x = 0; x < input.Width; x++)
            {
                var sum = 0;

                var c = new List<byte>();

                for (int y = 0; y < input.Height; y++)
                {
                    var pixel = input.GetMonochromPixel(x, y);
                    sum += pixel;
                    c.Add(pixel);
                }

                if (sum > b)
                {
                    if (currentRow == null)
                    {
                        currentRow = new List<List<byte>>();
                        isRow = true;
                    }

                    currentRow.Add(c.ToArray().Reverse().ToList());
                }
                else if (isRow && sum == 0)
                {
                    if (currentRow != null)
                    {
                        result.Add(Help(currentRow));
                        currentRow = null;
                    }
                }
            }

            return result.Select(t => t.ToBitmapFromMonochrom()).ToArray();
        }

        private Bitmap[] GetWords(Bitmap input)
        {
            List<byte[,]> result = new List<byte[,]>();

            List<List<byte>> currentRow = null;
            var whiteCellWidth = 1;
            var maxCellWidth = 8;

            for (int x = 0; x < input.Width; x++)
            {
                var sum = 0;

                var c = new List<byte>();

                for (int y = 0; y < input.Height; y++)
                {
                    var pixel = input.GetMonochromPixel(x, y);
                    sum += pixel;
                    c.Add(pixel);
                }

                if (sum == 0)
                {
                    whiteCellWidth = whiteCellWidth + 1;
                }
                else
                {
                    whiteCellWidth = 0;
                }

                if (whiteCellWidth < maxCellWidth)
                {
                    if (currentRow == null)
                    {
                        currentRow = new List<List<byte>>();
                    }

                    currentRow.Add(c.ToArray().Reverse().ToList());
                }
                else if (whiteCellWidth > maxCellWidth)
                {
                    whiteCellWidth = 0;

                    result.Add(Help(currentRow));
                    currentRow = null;
                }
            }

            if (currentRow != null)
            {
                result.Add(Help(currentRow));
            }

            return result.Select(t => t.ToBitmapFromMonochrom()).ToArray();
        }

        private byte[,] Help(List<List<byte>> list)
        {
            if (!list.Any())
            {
                return new byte[0, 0];
            }

            var width = list[0].Count;

            var result = new byte[width, list.Count];

            for (int y = 0; y < result.GetLength(1); y++)
            {
                var l = list[y];

                for (int x = 0; x < result.GetLength(0); x++)
                {
                    result[x, y] = l[x];
                }
            }

            return result;
        }

        private Bitmap Scale(Bitmap bitmap, int width, int height)
        {
            if (bitmap == null)
            {
                return null;
            }

            float floatWidth = width;
            float floatHeight = height;

            float scale = Math.Min(floatWidth / bitmap.Width, floatHeight / bitmap.Height);

            var bmp = new Bitmap((int)floatWidth, (int)floatHeight);


            var graph = Graphics.FromImage(bmp);

            var scaleWidth = (int)(bitmap.Width * scale);
            var scaleHeight = (int)(bitmap.Height * scale);

            graph.DrawImage(bitmap, ((int)floatWidth - scaleWidth) / 2, ((int)floatHeight - scaleHeight) / 2, scaleWidth, scaleHeight);

            return bmp;
        }
    }
}
