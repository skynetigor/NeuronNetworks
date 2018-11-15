using System;

namespace NeuronNetworks.Perseptron.Interfaces
{
    public interface INeuronNetwork
    {
        double[] GetAnswer(params double[] inputs);

        void Study((double[], double[])[] trainSets, int epochsCount = 5000, double learningRate = 0.1, Action<double[]> mse = null);
    }
}