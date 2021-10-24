namespace Pooling
{
    public interface IPoolDataSource
    {
        int GetItemCount();
        void SetCell(ICell cell, int index);
    }
}