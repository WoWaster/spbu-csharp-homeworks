namespace ConcurrentPriorityQueue;

using System.Collections;

/// <summary>Represents a thread-safe max priority queue.</summary>
/// <typeparam name="TElement">Specifies the type of elements in the queue.</typeparam>
/// <typeparam name="TPriority">Specifies the type of priority associated with enqueued elements.</typeparam>
public class ConcurrentPriorityQueue<TElement, TPriority>
{
    private readonly object lockObject = new();

    // By the task we need to dequeue element with highest priority, but by default PriorityQueue dequeues element with the lowest one
    // We hack that by reversing internal comparator
    private readonly PriorityQueue<TElement, TPriority> queue =
        new(Comparer<TPriority>.Create((a, b) => -Comparer.Default.Compare(a, b)));

    /// <summary>Gets the number of elements contained in the ConcurrentPriorityQueue.</summary>
    public int Size
    {
        get
        {
            lock (this.lockObject)
            {
                return this.queue.Count;
            }
        }
    }

    /// <summary>Gets a collection that enumerates the elements of the queue in an unordered manner.</summary>
    public PriorityQueue<TElement, TPriority>.UnorderedItemsCollection UnorderedItemsCollection
    {
        get
        {
            lock (this.lockObject)
            {
                return this.queue.UnorderedItems;
            }
        }
    }

    /// <summary>Adds the specified element with associated priority to the ConcurrentPriorityQueue.</summary>
    /// <param name="element">The element to add to the ConcurrentPriorityQueue.</param>
    /// <param name="priority">The priority with which to associate the new element.</param>
    public void Enqueue(TElement element, TPriority priority)
    {
        lock (this.lockObject)
        {
            this.queue.Enqueue(element, priority);
            Monitor.PulseAll(this.lockObject);
        }
    }

    /// <summary>
    ///     Removes and returns the maximal element from the ConcurrentPriorityQueue.
    ///     If ConcurrentPriorityQueue is empty, then it will block current thread waiting for element in the queue.
    /// </summary>
    /// <returns>The maximal element of the ConcurrentPriorityQueue.</returns>
    public TElement Dequeue()
    {
        lock (this.lockObject)
        {
            while (this.queue.Count == 0)
            {
                Monitor.Wait(this.lockObject);
            }

            var element = this.queue.Dequeue();
            Monitor.PulseAll(this.lockObject);
            return element;
        }
    }
}