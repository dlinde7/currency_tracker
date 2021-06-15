using System;

namespace currency_tracker.Utility
{
    public partial class ClassConverter
    {
        public static class Ratio
        {
            /// <summary>
            /// Calculate ratio as min/max = ratio
            /// </summary>
            /// <param name="A">Value 1</param>
            /// <param name="B">Value 2</param>
            /// <returns>The ratio</returns>
            public static double CalculateMin(double A, double B)
            {
                return Math.Max(A, B) / Math.Min(A, B);
            }

            /// <summary>
            /// Calculate ratio as max/min = ratio
            /// </summary>
            /// <param name="A">Value 1</param>
            /// <param name="B">Value 2</param>
            /// <returns>The ratio</returns>
            public static double CalculateMax(double A, double B)
            {
                return Math.Min(A, B) / Math.Max(A, B);
            }

            /// <summary>
            /// Calculate ratio as A/B = ratio
            /// </summary>
            /// <param name="A">Value 1</param>
            /// <param name="B">Value 2</param>
            /// <returns>The ratio</returns>
            public static double Calculate(double A, double B)
            {
                return A / B;
            }
        }
    }
}