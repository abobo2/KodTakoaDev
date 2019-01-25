using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Kernel
{
    private Dictionary<Type, object> container;

    public Kernel()
    {
        container = new Dictionary<Type, object>();
    }

    public void Add<T>(T instance)
        where T : class
    {
        container.Add(typeof(T), instance);
    }

    public T GetInstanceOfType<T>()
        where T : class
    {
        if (!container.ContainsKey(typeof(T)))
        {
            Debug.LogError("Instance not found of type: " + typeof(T).Name);
        }

        return container[typeof(T)] as T;
    }

    public List<T> GetInstancesOfType<T>()
        where T : class
    {
        List<T> list = container
            .Where(x => (x as T) != null)
            .Select(x => x as T)
            .ToList();

        if (list == null)
        {
            list = new List<T>();
        }

        return list;
    }
}
