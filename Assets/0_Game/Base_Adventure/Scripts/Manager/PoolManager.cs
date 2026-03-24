using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolItem<T> where T : MonoBehaviour
{
    public T prefab;
    public int startAmount = 10;

    [HideInInspector]
    public Queue<T> pool = new Queue<T>();
}

public class PoolManager : MonoBehaviour
{

    protected void InitPool<T>(PoolItem<T> poolItem) where T : MonoBehaviour
    {
        for (int i = 0; i < poolItem.startAmount; i++)
        {
            CreateObject(poolItem);
        }
    }

    T CreateObject<T>(PoolItem<T> poolItem) where T : MonoBehaviour
    {
        var obj = Instantiate(poolItem.prefab);
        obj.gameObject.SetActive(false);
        poolItem.pool.Enqueue(obj);
        return obj;
    }

    public T Get<T>(PoolItem<T> poolItem) where T : MonoBehaviour
    {
        if (poolItem.pool.Count == 0)
        {
            CreateObject(poolItem);
        }

        var obj = poolItem.pool.Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void Return<T>(PoolItem<T> poolItem, T obj) where T : MonoBehaviour
    {
        obj.gameObject.SetActive(false);
        poolItem.pool.Enqueue(obj);
    }
}
