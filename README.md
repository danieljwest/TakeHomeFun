# Introduction
Hi! Here is my solution to the distributed median problem. It's written in C# using .NET Core 2.0. 
The solution, start to finish, took me about 3.5-4 hours.
# Proposed Solution
We don't need to sort all of the arrays, we just need to determine which number is at the middle position across all sets. To do this we do the following:
* Find an approximate median by using the "Median of Medians" algorithm
* Determine if the approximate median is in the expected median position or if it's to the left or right.
* If it's to the right of the median, we do the same process but with only the numbers to the left of the median.
* If it's to the left of the median, we do the same process but with only the numbers to the right of the median.
* If it is in the median position, we return it... because we're there.
## Assumptions
I assumed a few things when writing this solution.
1. The arrays are not sorted
2. The arrays may contain duplicates
3. The arrays are made up of integers (this could be changed to support other numerics)
4. The median number should be a part of the set. If there is an even number of numbers, we will choose the lesser of the two middle numbers. (This could be turned into the average version by finding the first element greater than the found median from each node, sorting, picking the first, and averaging.)
5. We will know the total count of numbers (size of one array * number of nodes)
6. The nodes are separate processes and there is some protocol for communicating with the nodes.
7. The nodes can execute whatever code we want
8. Memory usage and performance is important
9. The nodes will be preloaded with the numbers
## Challenges
* The assumption that the nodes shouldn't load everything into memory makes for a little more verbose code than would otherwise be neccessary.
* My kids singing "Fight Song" on repeat while I coded the solution.
# Getting Started
* Download and install the .NET Core SDK [.NET Downloads for Windows](https://www.microsoft.com/net/download/windows)
* *dotnet build* - Compiles the project
* *dotnet test DistributedMedian.Tests* - Run all of the tests
# Projects
* DistributedMedian.Orchestrator - Main service responsbile for getting the information it needs from the nodes and calculating the actual median
* DistributedMedian.Node - Node service representive of a single node and the operations that can be performed on it
* DistributedMedian.Core - Holds interfaces and shared business logic
* DistributedMedian.Tests - Holds the tests for the projects.
# Improvements
More tests could be added to improve coverage with substitutes for other pieces.
A different repository could be implemented that isn't reliant on memory
There may be a more appropriate way to pick a pivot.
I also don't feel great about having to pivot the numbers on a node I could have left them there, but the algorithm would become less efficient.
