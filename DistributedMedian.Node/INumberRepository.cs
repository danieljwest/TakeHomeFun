using System.Collections.Generic;

namespace DistributedMedian.Node
{
    public interface INumberRepository
    {
        IEnumerable<int> GetNumbers();

        void SaveNumbers(IEnumerable<int> numbers);
    }
}