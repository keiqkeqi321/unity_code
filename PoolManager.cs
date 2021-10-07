using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PoolManager : MonoBehaviour
{
    [SerializeField] ObjectPool[] pools;
    static Dictionary<GameObject, ObjectPool> objDictionary;
    private void Update()
    {
        Debug.Log(pools[0].currentSize);
    }
    private void Start()
    {
        objDictionary = new Dictionary<GameObject, ObjectPool>();
        InitializePools();
    }
#if UNITY_EDITOR
    void OnDestroy()
    {
        CheckSize();
    }
#endif
    //���Ԥ������������
    void CheckSize()
    {
        foreach (var item in pools)
        {
            if (item.currentSize > item.Size)
            {
                Debug.LogWarning(string.Format("the {0} pool has the current size {1} which bigger then initial size{2}",
                    item.Prefab.name,
                    item.currentSize,
                    item.Size));
            }
        }
    }
    //��ʼ�������
    void InitializePools()
    {
        
        foreach(var pool in pools)
        {
#if UNITY_EDITOR
            if ( objDictionary.ContainsKey(pool.Prefab))
            {
                Debug.LogError("save the same prefab in multiple pool  ,prefabName:" + pool.Prefab.name);
                continue;
            }
#endif
            objDictionary.Add(pool.Prefab,pool);
            Transform parentsTransform= new GameObject("pool:" + pool.Prefab.name).transform;
            parentsTransform.parent = transform;
            pool.Initialize(parentsTransform);
        }
    }
    #region   releaseGameObj
    public static GameObject ReleaseGameObject( GameObject prefab,Vector3 position)
    {
#if UNITY_EDITOR
        if (!objDictionary.ContainsKey(prefab))
        {
            Debug.LogError("can not find this prefab in the pools");
            return null ;
        }
#endif
        return objDictionary[prefab].PreparedObject(position);
    }
    public static GameObject ReleaseGameObject(GameObject prefab, Vector3 position,Quaternion rotation)
    {
#if UNITY_EDITOR
        if (!objDictionary.ContainsKey(prefab))
        {
            Debug.LogError("can not find this prefab in the pools");
            return null;
        }
#endif
        return objDictionary[prefab].PreparedObject(position,rotation);
    }
    public static GameObject ReleaseGameObject(GameObject prefab, Vector3 position, Quaternion rotation,Vector3 scale)
    {
#if UNITY_EDITOR
        if (!objDictionary.ContainsKey(prefab))
        {
            Debug.LogError("can not find this prefab in the pools");
            return null;
        }
#endif
        return objDictionary[prefab].PreparedObject(position, rotation,scale);
    }
    #endregion  
    
}

#region �������
[System.Serializable]
public class ObjectPool
{
    [SerializeField] GameObject prefab;
    [SerializeField] int size = 1;
    Transform parent;
    Queue<GameObject> queue;//�ض���
    public GameObject Prefab => prefab;//���ж���
    public int Size => size;//Ԥ������
    public int currentSize => queue.Count;//��ǰ������
    //�������
    public void Initialize(Transform parent)
    {
        this.parent = parent;
        queue = new Queue<GameObject>();
        for (int i = 0; i < size; i++)
        {
            queue.Enqueue(Copy(this.parent));

        }
    }
    //��Ԥ��������������Ϊ������
    GameObject Copy(Transform parent)
    {
        var copy = GameObject.Instantiate(prefab);
        copy.transform.parent = parent;
        copy.SetActive(false);
        return copy;
    }
    //��ȡһ�����е�����
    GameObject AviailableObject()
    {
        GameObject availableObject = null;
        if (queue.Count > 0 && !queue.Peek().activeSelf) //���е�һ��Ԫ��û������
        {
            availableObject = queue.Dequeue();

        }
        else
        {
            availableObject = Copy(this.parent);
        }
        queue.Enqueue(availableObject);
        return (availableObject);
    }


    #region get prepared object  ����һ�����е����岢����
    public GameObject PreparedObject(Vector3 position)
    {

        GameObject prepare = AviailableObject();
        prepare.SetActive(true);
        prepare.transform.position = position;
        return prepare;
    }
    public GameObject PreparedObject(Vector3 position, Quaternion rotation)
    {
        GameObject prepare = AviailableObject();
        prepare.SetActive(true);
        prepare.transform.position = position;
        prepare.transform.rotation = rotation;
        return prepare;
    }
    public GameObject PreparedObject(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        GameObject prepare = AviailableObject();
        prepare.SetActive(true);
        prepare.transform.position = position;
        prepare.transform.rotation = rotation;
        prepare.transform.localScale = scale;
        return prepare;
    }
    #endregion
}
#endregion