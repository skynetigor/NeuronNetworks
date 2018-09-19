using System;
using NeuronNetworks.Perseptron.Interfaces;

namespace NeuronNetworks.Perseptron.Neurons
{
    public class Neuron: INeuron
    {
        static readonly Random random = new Random();

        public double[] Weights { get; }

        public ILayer CurrentLayer { get; set; }

        public Neuron(int weightLength, ILayer currentLayer)
        {
            Weights = new double[weightLength];

            for (int i = 0; i < weightLength; i++)
            {
                //Weights[i] = 0.5;
                Weights[i] = (double)1 / random.Next(2, 10);
            }

            CurrentLayer = currentLayer;
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
