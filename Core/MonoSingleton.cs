using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    protected static T _Instance;
    public static T Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindFirstObjectByType<T>(FindObjectsInactive.Include);
                _Instance?.OnCreateInstance();
            }
            return _Instance;
        }
    }

    protected virtual void OnCreateInstance() { }
}
