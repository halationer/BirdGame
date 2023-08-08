
using UnityEngine;

public class UISingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        Instance = this as T;
        Debug.Log(Instance);
    }
}
