namespace NeuronNetworks.Perseptron.Interfaces
{
    public interface IOutputLayer : ILayer
    {
        INeuron[] Neurons { get; }
        ILayer PreviousLayer { get; set; }
    }
}
