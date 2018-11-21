using System.Collections.Generic;
using NeuronNetworks;

namespace Example
{
    static class Extensions
    {
        public static void Add(this Dictionary<double[], KeyValuePair<double[], int>> dictionary, string first, string second, int expected)
        {
            dictionary.Add(first.ToDoubleArray(), new KeyValuePair<double[], int>(second.ToDoubleArray(), expected));
        }
    }
}
