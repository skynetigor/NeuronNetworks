namespace NeuronNetworks.Perseptron.Interfaces
{
    public interface INeuron
    {
        double[] Weights { get; }
        ILayer CurrentLayer { get; }
        int Index { get; }

        double Handle(double[] inputs);

    }
}
