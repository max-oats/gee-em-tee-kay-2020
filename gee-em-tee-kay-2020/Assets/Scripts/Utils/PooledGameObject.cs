using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PooledGameObject
{
    public PooledGameObject(GameObject go)
    {
        gameObject = go;
    }

    public bool inUse = false; /** Whether the pooled go is in use */

    public GameObject gameObject; /** The game object associated w this pool */
}

[System.Serializable]
public class GameObjectPool
{
    [SerializeField] private Transform transform = null;
    [SerializeField] private float poolSize = 64f;
    [SerializeField] private GameObject poolPfb = null;
    [SerializeField] private bool warnIfFull = true;

    private List<PooledGameObject> pool = new List<PooledGameObject>();

    public void Init()
    {
        for (int i = 0; i < poolSize; ++i)
        {
            AddToPool();
        }
    }

    public GameObject Instantiate()
    {
        if (pool.Count == 0)
        {
            Debug.LogWarningFormat("Object pool for prefab '{0}' has no pooled objects. Init() might have to be called before attempted to instantiate.", poolPfb.name);
            return null;
        }

        PooledGameObject go = pool.Find(x => !x.inUse);
        if (go == null)
        {
            if (warnIfFull)
            {
                Debug.LogWarningFormat("Object pool for prefab '{0}' has no available pooled objects. Increase size of pool?");
            }
            return null;
        }

        /** Found a usable object! */
        go.inUse = true;
        go.gameObject.SetActive(true);

        return go.gameObject;
    }

    public GameObject Instantiate(Vector3 position, Quaternion rotation)
    {
        GameObject go = Instantiate();

        go.transform.position = position;
        go.transform.rotation = rotation;

        return go;
    }

    public GameObject Instantiate(Transform newTransform)
    {
        GameObject go = Instantiate();

        go.transform.SetParent(newTransform);

        return go;
    }

    public GameObject Instantiate(Vector3 position, Quaternion rotation, Transform newTransform)
    {
        GameObject go = Instantiate();

        go.transform.SetParent(newTransform);
        go.transform.position = position;
        go.transform.rotation = rotation;

        return go;
    }

    private void Destroy(GameObject gameObject)
    {
        PooledGameObject go = pool.Find(x => x.gameObject.Equals(gameObject));

        go.gameObject.transform.SetParent(transform);
        go.gameObject.SetActive(false);
        go.inUse = false;
    }

    public void Destroy(MonoBehaviour mono, GameObject gameObject, float delay = -1f)
    {
        if (delay > 0f)
        {
            mono.StartCoroutine(DestroyWithDelay(gameObject, delay));
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator DestroyWithDelay(GameObject gameObject, float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(gameObject);
    }

    private void AddToPool()
    {
        GameObject go = GameObject.Instantiate(poolPfb, transform);
        go.SetActive(false);
        pool.Add(new PooledGameObject(go));
    }
}