using System.Collections.Generic;
using UnityEngine;

public static class ObjectPool
{
    #region Methods

    private static GameObject CreateAnInstance(GameObject prefab)
    {
        GameObject instance = Object.Instantiate(prefab);
        instance.name = prefab.name;

        return instance;
    }

    public static void PreLoadPool(GameObject prefab, int number)
    {
        for (int i = 0; i < number; i++)
        {
            GameObject instance = CreateAnInstance(prefab);
            AddToPool(instance);
        }
    }

    public static void AddToPool(GameObject objectToStore)
    {
        objectToStore.SetActive(false);

        if (_pool.ContainsKey(objectToStore.name))
        {
            _pool[objectToStore.name].Add(objectToStore);
        }
        else
        {
            List<GameObject> newList = new List<GameObject>();
            newList.Add(objectToStore);

            _pool.Add(objectToStore.name, newList);
        }
    }

    public static GameObject GetFromPool(GameObject prefab)
    {
        GameObject instance;

        if (!_pool.ContainsKey(prefab.name))
        {
            instance = CreateAnInstance(prefab);

            return instance;
        }
        else
        {
            if (_pool[prefab.name].Count > 0)
            {
                instance = _pool[prefab.name][0];
                instance.SetActive(true);
                _pool[prefab.name].RemoveAt(0);

                return instance;
            }
            else
            {
                instance = CreateAnInstance(prefab);

                return instance;
            }
        }
    }

    public static void EmptyPool()
    {
        _pool.Clear();
    }

    #endregion


    #region Private fields

    private static Dictionary<string, List<GameObject>> _pool = new Dictionary<string, List<GameObject>>();

    #endregion
}
