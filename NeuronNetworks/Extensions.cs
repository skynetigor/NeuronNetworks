using System.Linq;

namespace NeuronNetworks
{
    public static class Extensions
    {
        public static double[] ToDoubleArray(this string str)
        {
            //return str.Select(t => (double) t).ToArray();
            return str.Select(t => (double)100 / t).ToArray();
        }
    }
}
