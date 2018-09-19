using NeuronNetworks.Perseptron.Interfaces;

namespace NeuronNetworks.Perseptron.Layers
{
    public class HiddenLayer : OutputLayer, IHiddenLayer
    {
        public HiddenLayer(int neuronsCount, ILayer previousLayer, int index) : base(neuronsCount, previousLayer, index)
        {

        }

        public ILayer NextLayer { get; set; }

        public override double[] Handle(double[] inputs)
        {
            var neuronsResult = base.Handle(inputs);

            return NextLayer.Handle(neuronsResult);
        }
    }
}
