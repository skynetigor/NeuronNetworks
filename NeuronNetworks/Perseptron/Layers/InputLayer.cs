using NeuronNetworks.Perseptron.Interfaces;

namespace NeuronNetworks.Perseptron.Layers
{
    class InputLayer : IInputLayer
    {
        public InputLayer(int inputsCount, int index)
        {
            InputsCount = inputsCount;
            Index = index;
        }

        public int Index { get; }
        public ILayer NextLayer { get; set; }
        public int InputsCount { get; }

        public int OutputsCount => InputsCount;

        public InputOutputModel GetInputAndOutput(double[] inputs)
        {
            return new InputOutputModel(inputs, inputs);
        }

        public double[] Handle(double[] inputs)
        {
           return NextLayer.Handle(inputs);
        }
    }
}
