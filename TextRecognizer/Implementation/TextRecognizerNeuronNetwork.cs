using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NeuronNetworks.Perseptron;
using NeuronNetworks.Perseptron.Interfaces;

namespace TextRecognizer.Implementation
{
    public class TextRecognizerNeuronNetwork : INeuronNetwork
    {
        private readonly INeuronNetwork[] neuronNetworks;

        public TextRecognizerNeuronNetwork(int inputsCount, int outputsCount)
        {
            neuronNetworks = new INeuronNetwork[outputsCount];

            for (int i = 0; i < outputsCount; i++)
            {
                neuronNetworks[i] = new NeuronNetwork(new PerseptronConfig(new[] { inputsCount, 1 }));
            }
        }

        public double[] GetAnswer(params double[] inputs)
        {
            return neuronNetworks.Select(t => t.GetAnswer(inputs)[0]).ToArray();
        }

        public void Study((double[], double[])[] trainSets, int epochsCount, double learningRate, Action<double[]> mse = null)
        {
            var dictionary = new Dictionary<INeuronNetwork, (double[], double[])[]>();

            for (int i = 0; i < neuronNetworks.Length; i++)
            {
                var individualTrainSets = new(double[], double[])[trainSets.Length];

                for(int t = 0; t < individualTrainSets.Length; t++)
                {
                    individualTrainSets[t] = (trainSets[t].Item1, new double[] { trainSets[t].Item2[i] });
                }

                dictionary.Add(neuronNetworks[i] ,individualTrainSets);
            }
            var index = 0;
            Parallel.ForEach(dictionary, t =>
            {
                var cc = index;
                index++;
                t.Key.Study(t.Value, epochsCount, learningRate);
                Console.WriteLine($"Neuron {cc} has finished studying.");
            });
        }
    }
}
