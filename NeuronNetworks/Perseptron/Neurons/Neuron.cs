using System;
using NeuronNetworks.Perseptron.Interfaces;

namespace NeuronNetworks.Perseptron.Neurons
{
    public class Neuron: INeuron
    {
        static readonly Random random = new Random();

        public double[] Weights { get; }

        public ILayer CurrentLayer { get; set; }

        public int Index { get; }

        public Neuron(int weightLength, int index, ILayer currentLayer)
        {
            Weights = new double[weightLength];

            for (int i = 0; i < weightLength; i++)
            {
                Weights[i] = (double)1 / random.Next(2, 10);
            }

            CurrentLayer = currentLayer;
            Index = index;
        }

        public double Handle(params double[] inputs)
        {
            double power = 0;

            for(int i = 0; i < inputs.Length; i++)
            {
                power += inputs[i] * Weights[i];
            }

            return power.ThroughtSigmoid();
        }
    }
}
