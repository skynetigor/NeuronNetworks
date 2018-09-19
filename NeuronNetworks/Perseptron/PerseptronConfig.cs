namespace NeuronNetworks.Perseptron
{
    public class PerseptronConfig
    {
        public PerseptronConfig(int[] neuronsPerLayer)
        {
            NeuronsPerLayer = neuronsPerLayer;
        }

        public int[] NeuronsPerLayer { get; }
    }
}
