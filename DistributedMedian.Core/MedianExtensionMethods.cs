using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DistributedMedian.Core
{

    public static class MedianExtensionMethod 
    {
        public static int FindApproximateMedian(this IEnumerable<int> numbers)
        {
            //Using IEnumerable so that the entire list of numbers does not have to be brought into memory. 
            var mediansOfSplits = numbers.SplitEveryN(5).Select(split => split.SortAndGetMiddlePosition()).ToArray();

            if (mediansOfSplits.Count() <= 5)
            {
                return mediansOfSplits.SortAndGetMiddlePosition();
            }

            return FindApproximateMedian(mediansOfSplits);
        }
        
        public static int SortAndGetMiddlePosition(this int[] list)
        {
            var medianPosition = (list.Length -1) / 2;
            return list.OrderBy(n => n).Where((x, index) => index == medianPosition).First();
        }
        
        public static IEnumerable<int[]> SplitEveryN(this IEnumerable<int> numbers, int n)
        {
            //For some reason, C# doesn't have a nice method to do this
            //Using a generator to keep from loading everything into memory
            var index = 0;
            var subSplit = new List<int>();
            foreach (var num in numbers)
            {
                subSplit.Add(num);
                index++;
                if (index == n)
                {
                    yield return subSplit.ToArray();
                    index = 0;
                    subSplit = new List<int>();
                }
            }

            if (subSplit.Any())
            {
                yield return subSplit.ToArray();
            }
            
        }
    }
}