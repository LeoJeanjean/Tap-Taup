using UnityEngine;
using System.Collections.Generic;

public class ObjectPooling : MonoBehaviour
{
    private static ObjectPooling instance;
    private Dictionary<string, Queue<GameObject>> pooledObjects;

    public static ObjectPooling Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ObjectPooling>();
                if (instance == null)
                {
                    GameObject obj = new("ObjectPooling");
                    instance = obj.AddComponent<ObjectPooling>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        pooledObjects = new Dictionary<string, Queue<GameObject>>();
    }

    public void AddPoolObject(string key, GameObject obj)
    {
        if (obj == null)
        {
            Debug.LogError("Attempted to add a null object to the pool.");
            return;
        }
        
        obj.SetActive(false);
        if (!pooledObjects.ContainsKey(key))
        {
            pooledObjects[key] = new Queue<GameObject>();
        }
        pooledObjects[key].Enqueue(obj);
    }

    public GameObject GetPooledObject(string key)
    {
        if (pooledObjects.TryGetValue(key, out Queue<GameObject> objectQueue))
        {
            if (objectQueue.Count > 0)
            {
                GameObject pooledObject = objectQueue.Dequeue();
                return pooledObject;
            }
        }
        return null;
    }

}