using System;
using System.Collections.Generic;
using System.Linq;
using DistributedMedian.Core;

namespace DistributedMedian.Orchestrator
{
    public class MedianOrchestrator
    {
        /// <summary>
        /// Finds the median that exists within the dataset. If the dataset as an even number of elements it will pick
        /// the lower of the two.
        /// </summary>
        public int FindDistributedMedian(IList<INodeService> nodes, int sizeOfAllArrays, int currentLocation = 0)
        {
            int medianLocation = (sizeOfAllArrays - 1) / 2;
            var approximateMedian = nodes
                .Select(n => n.GetApproximateMedian())
                .FindApproximateMedian();
       
            (int left, int same, int right) = nodes.Select(n => n.GetItemCountAroundValue(approximateMedian)).Aggregate((agg,item) =>
            {
                agg.left += item.left;
                agg.same += item.same;
                agg.right += item.right;
                return agg;
            });
            left += currentLocation;
            
            
            if (left <= medianLocation && left + same > medianLocation)
            {
                // We got it!
                return approximateMedian;
            }

            if (medianLocation < left)
            {
                foreach (var node in nodes)
                {
                    node.Pivot(approximateMedian, GreaterOrLessThan.LessThan);
                }
            }
            else
            {
                currentLocation = left + same;
                foreach (var node in nodes)
                {
                
                    node.Pivot(approximateMedian, GreaterOrLessThan.Greater);
                }
            }

            return FindDistributedMedian(nodes.Where(n=> n.HasValues()).ToList(), sizeOfAllArrays, currentLocation);
        }
    }
}