using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    static T instance;
    public static T Instance
    {
        get
        {
            return instance;
        }
    }
    protected virtual void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = (T)this;
        }
        DontDestroyOnLoad(this);
    }
    public static bool Initialized
    {
        get
        {
            return (instance != null);
        }
    }
    protected virtual void OnDestory()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

}
