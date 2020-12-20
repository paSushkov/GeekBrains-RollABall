namespace Application.Scripts.Common.Interfaces
{
    public interface IObjectPool<T>
    {
        bool GetObject(out T obj);
        void ReturnObject(T obj);
    }
}