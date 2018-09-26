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
        private readonly IImageScaler imageScaler;
        private readonly IImageFilter[] imageFilters;

        public ImageProcessorImpl(Config config, IEnumerable<IImageFilter> imageFilters, IImageScaler imageScaler)
        {
            this.config = config;
            this.imageScaler = imageScaler;
            this.imageFilters = imageFilters.ToArray();
        }

        public Bitmap[][][] GetLetters(Bitmap input)
        {
            input = ThroughFilters(input);

            var result = GetRows(input);

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

                    return bcc.Select(x => imageScaler.Scale(Cut(x), config.LetterHeight, config.LetterWidth)).Where(bitmap => Filter(bitmap)).ToArray();
                }).ToArray();
            }).ToArray();
        }

        public bool Filter(Bitmap bitmap)
        {
            for(int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    if (bitmap.GetMonochromPixel(x, y) > 0) {
                        return true;
                    };
                }
            }

            return false;
        }

        public Bitmap[] GetRows(Bitmap input)
        {
            var result = SeparateWhiteSpaceY(input).Where(t => t.Height > 1).ToArray();

            if (result.Length == 0)
            {
                result = new Bitmap[] { input };
            }

            return result.Select(t =>
            {
                if (t.Height != config.LetterHeight)
                {
                    var scale = (double)config.LetterHeight / t.Height;
                    var width = t.Width * scale;
                    t = imageScaler.Scale(t, (int)width, (int)config.LetterHeight);
                }

                return t;
            }).ToArray();
        }

        public Bitmap[] GetWords(Bitmap input)
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

        public Bitmap Cut(Bitmap input)
        {
            var sas = Scale(input);
            //sas.Save("temp/000sdaj.jpg");
            return sas;
            //input.Save("temp/input.jpg");
            //Scale(input).Save("temp/scale.jpg");
            //var isObjectFound = false;
            //List<List<byte>> currentRow = new List<List<byte>>();

            //for (int y = 0; y < input.Height; y++)
            //{
            //    var sum = 0;

            //    var c = new List<byte>();

            //    for (int x = 0; x < input.Width; x++)
            //    {
            //        if (isObjectFound == false)
            //            sum += input.GetMonochromPixel(x, y);

            //        c.Add(input.GetMonochromPixel(x, y));
            //    }

            //    if (!isObjectFound && sum > 0)
            //    {
            //        isObjectFound = true;
            //    }
            //    if (isObjectFound)
            //    {
            //        currentRow.Add(c);
            //    }
            //}

            //var bcc = Help(currentRow);



            //currentRow = new List<List<byte>>();
            //bcc.ToBitmapFromMonochrom().Save("temp/hhh.jpg");
            //isObjectFound = false;

            //for (int y = bcc.GetLength(1) - 1; y >= 0; y--)
            //{
            //    var sum = 0;

            //    var c = new List<byte>();

            //    for (int x = 0; x < bcc.GetLength(1); x++)
            //    {
            //        if (isObjectFound == false)
            //            sum += bcc[x, y];
            //        else
            //            c.Add(bcc[x, y]);
            //    }

            //    if (!isObjectFound && sum > 0)
            //    {
            //        isObjectFound = true;
            //    }
            //    else if (isObjectFound)
            //    {
            //        currentRow.Add(c);
            //    }
            //}

            //bcc = Help(currentRow);
            //bcc.ToBitmapFromMonochrom().Save("temp/hhh.jpg");
            //currentRow = new List<List<byte>>();
            //isObjectFound = false;
            //for (int y = 0; y < bcc.GetLength(1); y++)
            //{
            //    var sum = 0;

            //    var c = new List<byte>();

            //    for (int x = 0; x < bcc.GetLength(0); x++)
            //    {
            //        if (isObjectFound == false)
            //            sum += bcc[x, y];
            //        else
            //            c.Add(bcc[x, y]);
            //    }

            //    if (!isObjectFound && sum > 0)
            //    {
            //        isObjectFound = true;
            //    }
            //    else if (isObjectFound)
            //    {
            //        currentRow.Add(c);
            //    }
            //}

            //bcc = Help(currentRow);
            //bcc.ToBitmapFromMonochrom().Save("temp/hhh.jpg");

            //currentRow = new List<List<byte>>();
            //isObjectFound = false;
            //for (int x = bcc.GetLength(0) - 1; x >= 0; x--)
            //{
            //    var sum = 0;

            //    var c = new List<byte>();

            //    for (int y = 0; y < bcc.GetLength(1); y++)
            //    {
            //        if (isObjectFound == false)
            //            sum += bcc[x, y];
            //        else
            //            c.Add(bcc[x, y]);
            //    }

            //    if (!isObjectFound && sum > 0)
            //    {
            //        isObjectFound = true;
            //    }
            //    else if (isObjectFound)
            //    {
            //        currentRow.Add(c);
            //    }
            //}

            //try
            //{
            //    bcc.ToBitmapFromMonochrom().Save("temp/hhh.jpg");
            //    return bcc.ToBitmapFromMonochrom();
            //}
            //catch
            //{
            //    return null;
            //}

            return input;
        }

        public Bitmap Scale(Bitmap input)
        {
            var bitmap = new Bitmap(config.LetterHeight, config.LetterWidth);

            for(int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    bitmap.SetPixel(x, y, Color.White);
                }
            }

            if(input.Width > config.LetterHeight)
            {
                var scale = input.Width / config.LetterHeight;
                input = imageScaler.Scale(input, input.Width * scale, input.Height * scale);
            }

            using (var gfx = Graphics.FromImage(bitmap))
            {
                var f = (bitmap.Width - input.Width) / 2;
                var c = (bitmap.Height - input.Height) / 2;
                gfx.DrawImage(input, new Rectangle(new Point(f, c), new Size(input.Width, input.Height)));
            }

            return bitmap;
        }

        public Bitmap ThroughFilters(Bitmap input)
        {
            foreach(var filter in imageFilters)
            {
                input = filter.Handle(input);
            }

            return input;
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
    }
}
