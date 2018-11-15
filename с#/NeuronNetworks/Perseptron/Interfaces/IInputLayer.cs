namespace NeuronNetworks.Perseptron.Interfaces
{
    public interface IInputLayer : ILayer
    {
        ILayer NextLayer { get; set; }
    }
}
