using System;

namespace NeuronNetworks
{
    public static class Static
    {
        public static double ThroughtSigmoid(this double x)
        {
            return 1 / (1 + Math.Pow(Math.Exp(1), -x));
        }

        public static double ThroughWeightsDelta(this double x)
        {
            return ThroughtSigmoid(x) * (1 - ThroughtSigmoid(x));
        }

        public static double CoincidenceRate(char[] first, char[] second, Func<char, char, bool> func)
        {
            int power = 0;

            int maxLength = 0;

            if (first.Length > second.Length)
            {
                maxLength = first.Length;
            }
            else if (second.Length > first.Length || second.Length == first.Length)
            {
                maxLength = second.Length;
            }

            for (int i = 0; i < maxLength; i++)
            {
                if(i < first.Length && i < second.Length)
                {
                    break;
                }

                if (func(first[i], second[i]))
                {
                    power++;
                }
            }

            var result = ((double)power / maxLength);

            return result;
        }

        public static double[] Substruct(this double[] firstMatrix, double[] secondMatrix)
        {
            if(firstMatrix.Length != secondMatrix.Length)
            {
                throw new InvalidOperationException("Length must be equal");
            }

            var result = new double[firstMatrix.Length];

            for(int i = 0; i < result.Length; i++)
            {
                result[i] = firstMatrix[i] - secondMatrix[i];
            }

            return result;
        }

        public static double[] MultipleOn(this double[] matrix, Func<double, double> func)
        {
            var result = new double[matrix.Length];

            for(int i = 0; i < matrix.Length; i++)
            {
                result[i] = matrix[i] * func(matrix[i]);
            }

            return result;
        }
    }
}
