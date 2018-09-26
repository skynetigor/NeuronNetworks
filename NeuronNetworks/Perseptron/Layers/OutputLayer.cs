using System.Linq;
using NeuronNetworks.Perseptron.Interfaces;
using NeuronNetworks.Perseptron.Neurons;

namespace NeuronNetworks.Perseptron.Layers
{
    public class OutputLayer : IOutputLayer
    {
        public OutputLayer(int neuronsCount, ILayer previousLayer, int index)
        {
            Neurons = new INeuron[neuronsCount];
            var inputsCount = previousLayer.OutputsCount;

            for (int i = 0; i < Neurons.Length; i++)
            {
                Neurons[i] = new Neuron(inputsCount, i, this);
            }

            InputsCount = inputsCount;
            PreviousLayer = previousLayer;
            Index = index;
        }

        public int Index { get; }

        public INeuron[] Neurons { get; }

        public ILayer PreviousLayer { get; set; }

        public int InputsCount { get; }

        public int OutputsCount => Neurons.Length;

        public InputOutputModel GetInputAndOutput(double[] inputs)
        {
            return new InputOutputModel(inputs, Neurons.Select(t => t.Handle(inputs)).ToArray());
        }

        public virtual double[] Handle(double[] inputs)
        {
            return Neurons.Select(t => t.Handle(inputs)).ToArray();
        }

        public virtual void Study(double[] errors, double[][] inputsPerLayer, double[][] outputsPerLayer, double learningRate)
        {
            var wd = new double[Neurons.Length];

            for (int neuronIndex = 0; neuronIndex < this.Neurons.Length; neuronIndex++)
            {
                var neuron = this.Neurons[neuronIndex];

                double weightsDelta = errors[neuronIndex] * errors[neuronIndex].ThroughWeightsDelta();
                wd[neuronIndex] = weightsDelta;
                for (int weightIndex = 0; weightIndex < neuron.Weights.Length; weightIndex++)
                {
                    neuron.Weights[weightIndex] = neuron.Weights[weightIndex] - outputsPerLayer[this.PreviousLayer.Index][weightIndex] * weightsDelta * learningRate;
                }
            }

            var prevLayer = PreviousLayer as HiddenLayer;

            if (prevLayer != null)
            {
                var outputErrors = new double[prevLayer.OutputsCount];

                for (int i = 0; i < outputErrors.Length; i++)
                {
                    for(int j = 0; j < Neurons.Length; j++)
                    {
                        outputErrors[i] += Neurons[j].Weights[i] * wd[j];
                    }
                }

                prevLayer.Study(outputErrors, inputsPerLayer, outputsPerLayer, learningRate);
            }
        }
    }
}
