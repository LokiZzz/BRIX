namespace BRIX.Utility.Models
{
    public class LimitedQueue<T>(int limit) : Queue<T>(limit)
    {
        public int Limit { get; set; } = limit;

        public new void Enqueue(T item)
        {
            while (Count >= Limit)
            {
                Dequeue();
            }

            base.Enqueue(item);
        }
    }
}
