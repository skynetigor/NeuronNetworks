namespace NeuronNetworks.Perseptron.Interfaces
{
    public interface INeuron
    {
        double[] Weights { get; }

        double Handle(double[] inputs);

        ILayer CurrentLayer { get; }
    }
}
