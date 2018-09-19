using System;
using System.Collections.Generic;
using System.Linq;
using NeuronNetworks;
using NeuronNetworks.Perseptron;
using Newtonsoft.Json;
using System.Drawing;
using System.IO;
using RestSharp;
using System.Text;
using System.Buffers.Text;

namespace Example
{
    class Program
    {
        static NeuronNetwork network = new NeuronNetwork(new PerseptronConfig(new[] { 3, 2, 1 }));
        static RestClient restClient = new RestClient();

        public static void Main(string[] args)
        {
            //GetImage("https://slotoking.com/uploads/symbols/symbol_269645_746552_244476_ru_smbl-tropical-fruit-5.jpg");
            Dictionary<double[], KeyValuePair<double[], int>> d = new Dictionary<double[], KeyValuePair<double[], int>>();

            network.Study(new[] {
                (new double[] { 0, 0, 0 }, new double[] { 0 }),
                (new double[] { 0, 0, 1 }, new double[] { 1}),
                (new double[] { 0, 1, 0 }, new double[] { 0}),
                (new double[] { 0, 1, 1 }, new double[] { 0}),
                (new double[] { 1, 0, 0 }, new double[] { 1}),
                (new double[] { 1, 0, 1 }, new double[] { 1}),
                (new double[] { 1, 1, 0 }, new double[] { 0}),
                (new double[] { 1, 1, 1 }, new double[] { 0}),


            }, mse: ds => {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine($"{ds[0]}           ");
            });

            Check(new double[] { 0, 0, 0 });
            Check(0, 0, 1);
            Check(0, 1, 0);
            Check(0, 1, 1);
            Check(1, 0, 0);
            Check(1, 0, 1);
            Check(1, 1, 0);
            Check(1, 1, 1);

            Console.WriteLine();

            Console.ReadLine();
        }

        public static void Check(params double[] inputs)
        {

            var answers = network.GetAnswer(inputs);

            var result = new bool[answers.Length];

            for (int i = 0; i < result.Length; i++)
                result[i] = answers[i] > 0.5 ? true : false;

            Console.WriteLine($"Inputs - {JsonConvert.SerializeObject(inputs)}    Bool - {JsonConvert.SerializeObject(result)}    Original - {JsonConvert.SerializeObject(answers)}");
        }


    }
}
