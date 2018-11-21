namespace NeuronNetworks.Perseptron.Layers
{
    public class InputOutputModel
    {
        public InputOutputModel(double[] inputs, double[] outputs)
        {
            Inputs = inputs;
            Outputs = outputs;
        }

        public double[] Inputs { get; }

        public double[] Outputs { get; }
    }
}
