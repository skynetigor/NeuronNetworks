using System;
using System.Linq;
using NeuronNetworks.Perseptron.Interfaces;
using NeuronNetworks.Perseptron.Layers;

namespace NeuronNetworks.Perseptron
{
    public class NeuronNetwork : INeuronNetwork
    {
        const double DefaultLearningRate = 0.1;
        const int DefaultEpochCount = 5000;
        internal ILayer[] Layers { get; }

        internal IInputLayer InputLayer;

        private IOutputLayer outputLayer;

        public NeuronNetwork(PerseptronConfig perseptronConfig)
        {
            Layers = new ILayer[perseptronConfig.NeuronsPerLayer.Length];

            Layers[0] = InputLayer = new InputLayer(perseptronConfig.NeuronsPerLayer[0], 0);

            if (perseptronConfig.NeuronsPerLayer.Length > 1)
            {
                for (int i = 1; i < perseptronConfig.NeuronsPerLayer.Length; i++)
                {
                    ILayer previousLayer = Layers[i - 1];

                    if (i == perseptronConfig.NeuronsPerLayer.Length - 1)
                    {
                        Layers[i] = outputLayer = new OutputLayer(perseptronConfig.NeuronsPerLayer[i], previousLayer, i);
                    }
                    else
                    {
                        Layers[i] = new HiddenLayer(perseptronConfig.NeuronsPerLayer[i], previousLayer, i);
                    }
                }

                for (int i = 0; i < Layers.Length; i++)
                {
                    var layer = Layers[i] as IInputLayer;

                    if (layer != null)
                    {
                        layer.NextLayer = Layers[i + 1];
                    }
                }
            }
        }

        public double[] GetAnswer(params double[] inputs)
        {
            if (inputs.Length != InputLayer.InputsCount)
            {
                throw new InvalidOperationException($"Inputs length should be equal {InputLayer.InputsCount}");
            }

            return InputLayer.Handle(inputs);
        }

        public void Study((double[], double[])[] trainSets, int epochsCount, double learningRate, Action<double[]> mse = null)
        {
            for (int epochIndex = 0; epochIndex < epochsCount; epochIndex++)
            {

                foreach (var trainSet in trainSets)
                {
                    var inputs = trainSet.Item1;
                    var expected = trainSet.Item2;

                    var inputsPerLayer = new double[Layers.Length][];
                    var outputsPerLayer = new double[Layers.Length][];

                    for (int layerIndex = 0; layerIndex < Layers.Length; layerIndex++)
                    {
                        var layer = Layers[layerIndex];

                        var res = layer.GetInputAndOutput(layerIndex - 1 < 0 ? inputs : outputsPerLayer[layerIndex - 1]);

                        inputsPerLayer[layerIndex] = res.Inputs;
                        outputsPerLayer[layerIndex] = res.Outputs;
                    }

                    double[] errors = new double[outputLayer.Neurons.Length];

                    for (int neuronsindex = 0; neuronsindex < outputLayer.Neurons.Length; neuronsindex++)
                    {
                        errors[neuronsindex] = outputsPerLayer[outputLayer.Index][neuronsindex] - expected[neuronsindex];
                    }

                    mse?.Invoke(errors.Select(t => Math.Pow(t, 2)).ToArray());

                    (outputLayer as OutputLayer).Study(errors, inputsPerLayer, outputsPerLayer, learningRate);

                }

            }


        }
    }
}
