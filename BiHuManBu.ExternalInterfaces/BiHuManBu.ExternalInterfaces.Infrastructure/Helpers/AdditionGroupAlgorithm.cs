using System.Collections.Generic;

namespace BiHuManBu.ExternalInterfaces.Infrastructure.Helpers
{
    public static class AdditionGroupAlgorithm
    {
        public static List<long> GetGroups(long[] numbers)
        {
            var t = new List<long>();
            var result = new int[numbers.Length];
            Backtrace(0, result, numbers, t);
            return t;
        }

        private static void Backtrace(int i, int[] result, long[] numbers, List<long> t)
        {
            if (i >= numbers.Length)
            {
                return;
            }
            result[i] = 1; //选中当前数字

            PrintResult(result, numbers, t); //输出结果

            Backtrace(i + 1, result, numbers, t); //选择下一数字

            result[i] = 0; //剔除当前数字   

            Backtrace(i + 1, result, numbers, t); //选择下一数字
        }

        private static void PrintResult(int[] result, long[] numbers, List<long> t)
        {
            var count = 0;
            long total = 0;
            for (var i = 0; i < result.Length; i++)
            {
                count += result[i];

                if (result[i] == 1)
                {
                    total += numbers[i];
                }
            }
            t.Add(total);
        }
    }
}