using System.Collections.Generic;
using System.Linq;
using DistributedMedian.Node;

namespace DistributedMedian.Tests
{
    /// <summary>
    /// In a different scenario, this could be ran row by row from a file or some other repository
    /// </summary>
    public class InMemoryNumberRepository : INumberRepository
    {
        private int[] numbers;
        
        public InMemoryNumberRepository()
        {
            this.numbers = new int[0];
        }
        public InMemoryNumberRepository(int[] numbers)
        {
            this.numbers = numbers;
        }
        
        public IEnumerable<int> GetNumbers()
        {
            return numbers;
        }

        public void SaveNumbers(IEnumerable<int> numbers)
        {
            this.numbers = numbers.ToArray();
        }
    }
}