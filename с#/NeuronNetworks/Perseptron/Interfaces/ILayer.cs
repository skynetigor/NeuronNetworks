using NeuronNetworks.Perseptron.Layers;

namespace NeuronNetworks.Perseptron.Interfaces
{
    public interface ILayer
    {
        int Index { get; }
        int InputsCount { get; }
        int OutputsCount { get; }
        double[] Handle(double[] inputs);
        InputOutputModel GetInputAndOutput(double[] inputs);
    }
}
