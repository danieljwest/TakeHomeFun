using System;
using System.Collections.Generic;
using System.Linq;
using DistributedMedian.Core;
using DistributedMedian.Node;
using DistributedMedian.Orchestrator;
using Xunit;

namespace DistributedMedian.Tests
{
    public class MedianOrchestratorFunctionalTests
    {
        private MedianOrchestrator orchestrator;
        public MedianOrchestratorFunctionalTests()
        {
            orchestrator = new MedianOrchestrator();
        }
        [Fact]
        public void AllNumbersAreTheSame()
        {
            (IList<INodeService> nodes, int size) = StageDataFromString(
                "1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1", 5);
            Assert.True(orchestrator.FindDistributedMedian(nodes, size) == 1);
        }
        
        [Fact]
        public void FindMiddleNumberWithEvenNumberOfElements()
        {
            (IList<INodeService> nodes, int size) = StageDataFromString(
                "1 2 3 4 5 6 7 8 9 10", 2);
            Assert.True(orchestrator.FindDistributedMedian(nodes, size) == 5);
        }
        
        
        [Fact]
        public void FindMiddleNumberWithOddNumberOfElements()
        {
            (IList<INodeService> nodes, int size) = StageDataFromString(
                "1 2 3 4 5 6 7 8 9 10 11", 2);
            Assert.True(orchestrator.FindDistributedMedian(nodes, size) == 6);
        }
        
        
        [Fact]
        public void FindMedianWithOneElementAndOneNode()
        {
            (IList<INodeService> nodes, int size) = StageDataFromString(
                "5", 1);
            Assert.True(orchestrator.FindDistributedMedian(nodes, size) == 5);
        }
        
        [Fact]
        public void Generate10001Numbers()
        {
            var numbers = Enumerable.Range(0, 10001).ToList();
            var actualMedian = numbers.OrderBy(a => a).Skip(5000).First();

            (IList<INodeService> nodes, int size) = StageData(numbers, 20);
            
            Assert.True(orchestrator.FindDistributedMedian(nodes, size) == actualMedian);
        }
        
        /// <summary>
        /// Normally I wouldn't write a test that's not deterministic. There's nothing more annoying than a test that fails
        /// *sometimes*. But in the spirit of keeping it short I figured this would be fine.
        /// </summary>
        [Fact]
        public void GenerateRandomNumbers()
        {
            var random = new Random();
            for (var i = 0; i < 10; i++)
            {
                var numbers = Enumerable.Range(0, 100001).Select(n => random.Next()).ToArray();
                var actualMedian = numbers.OrderBy(a => a).Skip(50000).First();

                (IList<INodeService> nodes, int size) = StageData(numbers, 100);

                Assert.True(orchestrator.FindDistributedMedian(nodes, size) == actualMedian);
            }
        }
        
        /// <summary>
        /// Normally I wouldn't write a test that's not deterministic. There's nothing more annoying than a test that fails
        /// *sometimes*. But in the spirit of keeping it short I figured this would be fine.
        /// </summary>
        [Fact]
        public void GenerateRandomNumbersEven()
        {
            var random = new Random();
            for (var i = 0; i < 10; i++)
            {
                var numbers = Enumerable.Range(0, 100000).Select(n => random.Next()).ToArray();
                var actualMedian = numbers.OrderBy(a => a).Skip(49999).First();

                (IList<INodeService> nodes, int size) = StageData(numbers, 100);

                Assert.True(orchestrator.FindDistributedMedian(nodes, size) == actualMedian);
            }
        }


        private (IList<INodeService>, int) StageDataFromString(string numbers, int numberOfNodes)
        {

            var numberArray = numbers.Split(" ").Select(int.Parse);
            return StageData(numberArray, numberOfNodes);
        }
        
        private (IList<INodeService>, int) StageData(IEnumerable<int> numberArray, int numberOfNodes)
        {
         
            var numbersPerNode = (int)Math.Ceiling(numberArray.Count() / (decimal)numberOfNodes);
            return (numberArray
                .SplitEveryN(numbersPerNode)
                .Select(array =>
                {
                    var repo = new InMemoryNumberRepository(array);
                    return (INodeService)new NodeService(repo);
                }).ToList(), numberArray.Count());
        }
    }
}