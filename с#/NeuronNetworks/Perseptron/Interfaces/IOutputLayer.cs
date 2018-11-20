namespace NeuronNetworks.Perseptron.Interfaces
{
    public interface IOutputLayer : ILayer
    {
        INeuron[] Neurons { get; }
        ILayer PreviousLayer { get; set; }

        void Study(double[] errors, double[][] inputsPerLayer, double[][] outputsPerLayer, double learningRate);
    }
}
