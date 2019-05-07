using System;

namespace CurrencyHandler.Models.Extensions
{
    public static class ArrayExtension
    {
        /// <summary>
        /// splits a one-dimensional array into two-dimensional (with rows and columns) 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">one-dimensional array to be split</param>
        /// <param name="columnsLimit">max amount of columns</param>
        /// <returns></returns>
        public static T[][] Split<T>(this T[] data, int columnsLimit = 6)
        {
            // columns = sqrt(data.length)
            var columns = data.Length < Math.Pow(columnsLimit, 2) ? (int)Math.Sqrt(data.Length) : columnsLimit;

            var rows = data.Length / columns;

            var lastRow = data.Length % columns;

            if (lastRow == 0)
            {
                lastRow = rows;
            }
            else
            {
                columns++;
            }

            var result = new T[columns][];

            int current = 0;

            for (int i = 0; i < result.Length - 1; i++)
            {
                result[i] = new T[rows];

                for (int j = 0; j < result[i].Length; j++)
                {
                    result[i][j] = data[current];
                    current++;
                }
            }

            var lastIndex = result.Length - 1;

            result[lastIndex] = new T[lastRow];

            for (int j = 0; j < result[lastIndex].Length; j++)
            {
                result[lastIndex][j] = data[current];
                current++;
            }

            return result;
        }
    }
}
