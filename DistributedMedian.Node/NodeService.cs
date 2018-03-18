using System;
using System.Linq;
using System.Runtime.CompilerServices;
using DistributedMedian.Core;

namespace DistributedMedian.Node
{
    public class NodeService : INodeService
    {
        // You may notice that I have no implementations for numberRepository in the Node project.
        // Since I'm only running tests and simulating this scenario, I'm only using the InMemoryRepository 
        private INumberRepository numberRepository;
        public NodeService(INumberRepository numberRepository)
        {
            this.numberRepository = numberRepository;
        }
        
        /// <summary>
        /// Used to get a reasonable pivot for the set of data
        /// </summary>
        /// <returns></returns>
        public int GetApproximateMedian()
        {
            return numberRepository.GetNumbers().FindApproximateMedian();
        }

        /// <summary>
        /// Pivots the numbers greater than or less than
        /// </summary>
        /// <param name="x">Number to pivot around</param>
        /// <param name="direction">Pivot greater than or less than</param>
        public void Pivot(int x, GreaterOrLessThan direction)
        {
            var numbers = numberRepository.GetNumbers();
            
            switch (direction)
            {
                case GreaterOrLessThan.LessThan:
                    numberRepository.SaveNumbers(numbers.Where(n => n < x));
                    break;
                case GreaterOrLessThan.Greater:
                    numberRepository.SaveNumbers(numbers.Where(n => n > x));
                    break;
            }
        }

        public bool HasValues()
        {
            return numberRepository.GetNumbers().Any();
        }

        /// <summary>
        /// In order to determine what position x is at, we'll need to know how many numbers are less than or greater
        /// than as well as how many duplicates we have
        /// </summary>
        /// <param name="x"></param>
        /// <returns>number of elements surrounding the number x</returns>
        public (int left, int same, int right)  GetItemCountAroundValue(int x)
        {
            (int left, int same, int right) seed = (0, 0, 0);
            
            return numberRepository.GetNumbers().Aggregate(seed, (agg, n) =>
            {
                if (n < x)
                {
                    agg.left++;
                }
                else if (n > x)
                {
                    agg.right++;
                }
                else
                {
                    agg.same++;
                }

                return agg;
            });
        }

    
        
    }
}
