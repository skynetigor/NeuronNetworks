using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuronNetworks.Perseptron;
using Newtonsoft.Json;
using RestSharp;

namespace ExampleNetFramework
{
    class Program
    {
        static NeuronNetwork network = new NeuronNetwork(new PerseptronConfig(new[] { 2500, 5 }));

        private static List<(double[], double[])> listToStydy = new List<(double[], double[])>();

        static Dictionary<string, int> dict = new Dictionary<string, int>() {
            { "s", 1 },
            { "r", 2 },
            { "c", 3 },
            { "t", 4 },
            { "m", 5 },
        };

        static void Main(string[] args)
        {
            ToStudy("dd", "http://psand.ru/wp-content/uploads/2012/05/inversija_vydelenija.jpg");
            ToStudy("dd", "http://vseznayko.yolasite.com/resources/Doman_kartki_kolor.jpg");
            ToStudy("dd", "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTGwLOo0bwYPFRsNFLWFRIQtT-P49zJzo5wOjwaMTwxCssTjUsEjw");
            ToStudy("dd", "https://www.liketka.com/images/2013/november-2012/cvetovedenie/black-and-wite.jpg");
            ToStudy("s", "https://pics.livejournal.com/o_anna/pic/000dah68");
            ToStudy("s", "https://pics.livejournal.com/o_anna/pic/000d58zd");
            ToStudy("s", "https://pics.livejournal.com/o_anna/pic/000d62pt");
            ToStudy("c", "https://images.freeimages.com/images/premium/previews/3181/31816294-vector-creative-circle-flag-on-white-background-eps10.jpg");
            ToStudy("c", "http://nnosova.ru/wp-content/uploads/2014/01/chernyiy-krug.jpg");
            ToStudy("c", "https://vk.vkfaces.com/638421/v638421665/42b98/PyhdrgP46go.jpg?ava=1");
            ToStudy("c", "https://media.istockphoto.com/vectors/vector-red-circle-icon-on-white-background-vector-id598676304");
            ToStudy("c", "https://st3.depositphotos.com/9842262/15911/i/1600/depositphotos_159110844-stock-photo-yellow-watercolor-circle-watercolour-stain.jpg");
            ToStudy("c", "https://st2.depositphotos.com/3994049/11420/v/950/depositphotos_114201764-stock-illustration-circle-green-frame-with-grass.jpg");
            ToStudy("c", "https://st2.depositphotos.com/6831718/9825/v/950/depositphotos_98251144-stock-illustration-notebook-icon-on-white-background.jpg");
            ToStudy("c", "https://st.depositphotos.com/1005920/1623/i/950/depositphotos_16234243-stock-photo-earth-orange-glossy-circle-icon.jpg");
            ToStudy("r", "https://bipbap.ru/wp-content/uploads/2018/01/Japanse-vlag-1-1170x1170-640x640.png");
            ToStudy("r", "https://www.graycell.ru/picture/big/romb.jpg");
            ToStudy("r", "https://s00.yaplakal.com/pics/pics_original/9/3/7/10863739.png");
            ToStudy("t", "http://nnosova.ru/wp-content/uploads/2014/01/chernyiy-treugolnik.jpg");
            ToStudy("t", "https://photoshop-master.ru/articles/art220/59.jpg");
            ToStudy("m", "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTUNKxE50xZFytYmfCJgMmAhbS-njk5G1Yt_4_II6JGp377aOucvw");
            ToStudy("m", "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTUNKxE50xZFytYmfCJgMmAhbS-njk5G1Yt_4_II6JGp377aOucvw");

            network.Study(listToStydy.ToArray(), mse: d => {
                var c = JsonConvert.SerializeObject(d);
                //Console.Clear();
                Console.SetCursorPosition(0, 0);
                Console.WriteLine($"{c}                                                                       ");
            });


            while (true)
            {
                Console.Write("Path: ");
                string path = Console.ReadLine();

                CheckAnswer(path);

                Console.WriteLine();
            }
        }

        private static void ToStudy(string code, string path)
        {
            var input = new double[] { 0, 0, 0, 0, 0 };

            if(dict.TryGetValue(code, out int v)) {
                input[v-1] = 1;
            }

            listToStydy.Add((GetImage(path), input));
        }

        private static void CheckAnswer(string path)
        {
            var answer = network.GetAnswer(GetImage(path));

            Console.WriteLine(JsonConvert.SerializeObject(answer.Select(t => t > 0.5 ? true : false)));
        }

        public static double[] GetImage(string path)
        {
            var restClient = new RestClient(path);
            var response = restClient.Execute<List<byte>>(new RestRequest(Method.GET));

            Image image = Image.FromStream(new MemoryStream(response.RawBytes));

            float width = 50;
            float height = 50;
            var brush = new SolidBrush(Color.Black);

            float scale = Math.Min(width / image.Width, height / image.Height);

            var bmp = new Bitmap((int)width, (int)height);
            var graph = Graphics.FromImage(bmp);

            var scaleWidth = (int)(image.Width * scale);
            var scaleHeight = (int)(image.Height * scale);

            graph.FillRectangle(brush, new RectangleF(0, 0, width, height));
            graph.DrawImage(image, ((int)width - scaleWidth) / 2, ((int)height - scaleHeight) / 2, scaleWidth, scaleHeight);

            var buffer = new List<double>();

            for (int i = 0; i < bmp.Height; i++)
            {
                for (int j = 0; j < bmp.Width; j++)
                {
                    Color pixel = bmp.GetPixel(i, j);
                    var b = pixel.R >= 230 && pixel.G >= 230 && pixel.B >= 230 ? 0 : 1;

                    //if (b == 1)
                    //{
                    //    Console.ForegroundColor = ConsoleColor.Green;
                    //}
                    //else
                    //{
                    //    Console.ForegroundColor = ConsoleColor.White;
                    //}

                    //Console.Write("0 ");

                    buffer.Add(b);
                }
                Console.Write("\n");
            }

            Console.WriteLine();
            Console.ResetColor();

            return buffer.ToArray();
        }
    }
}
