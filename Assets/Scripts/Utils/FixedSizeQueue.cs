using System.Collections.Generic;

public class FixedSizedQueue<T> {
    private readonly object _privateLockObject = new object();
    private readonly Queue<T> _queue = new Queue<T>();
    private T _lastAdded;
    public int Size { get; private set; }

    public FixedSizedQueue(int size) {
        this.Size = size;
    }

    public void Enqueue(T obj) {
        lock (this._privateLockObject) {
            this._lastAdded = obj;
            this._queue.Enqueue(obj);
        }

        lock (this._privateLockObject) {
            while (this._queue.Count > this.Size) {
                this._queue.Dequeue();
            }
        }
    }

    public T Peek() {
        lock (this._privateLockObject) {
            return this._lastAdded;
        }
    }
}