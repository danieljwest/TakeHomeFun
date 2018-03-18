namespace DistributedMedian.Core
{
    public interface INodeService
    {
        int GetApproximateMedian();

        void Pivot(int x, GreaterOrLessThan direction);

        bool HasValues();

        (int left, int same, int right) GetItemCountAroundValue(int x);
    }
    
    public enum GreaterOrLessThan
    {
        Greater,
        LessThan
    }
}