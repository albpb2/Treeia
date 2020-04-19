using UnityEngine;

public class Singleton<TInstance> : MonoBehaviour where TInstance : MonoBehaviour
{
    private static TInstance _instance;

    public static TInstance Instance => _instance;

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as TInstance;
            DontDestroyOnLoad(_instance);
        }
        else
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}