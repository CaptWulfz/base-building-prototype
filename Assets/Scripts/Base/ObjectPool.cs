using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T: new()
{
    public delegate T CreatePoolable();
    private Stack<T> pool;
    private int maxPoolSize;
    private int currPoolSize;

    public ObjectPool(int maxPoolSize)
    {
        this.pool = new Stack<T>();
        this.maxPoolSize = maxPoolSize;
    }

    public T Get(CreatePoolable createFunc)
    {
        T poolable = default(T);
        if (this.pool.Count == 0)
        {
            if (this.currPoolSize < this.maxPoolSize)
            {
                poolable = createFunc.Invoke();
                this.currPoolSize++;
            }
        } else
        {
            poolable = pool.Pop();
        }

        return poolable;
    }

    public void Release(T item, Action beforeRelease, Action onRelease = null)
    {
        if (this.pool.Count < this.maxPoolSize)
        {
            beforeRelease.Invoke();
            this.pool.Push(item);
        }

        onRelease?.Invoke();
    }
}
