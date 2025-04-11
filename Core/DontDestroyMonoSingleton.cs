using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyMonoSingleton<T> : MonoBehaviour where T : DontDestroyMonoSingleton<T>
{
    protected static T _Instance;
    public static T Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindFirstObjectByType<T>(FindObjectsInactive.Include);
                DontDestroyOnLoad(_Instance.gameObject);
                _Instance.OnCreateInstance();
            }
            return _Instance;
        }
    }

    protected virtual void OnCreateInstance() { }

    protected virtual void Awake()
    {
        if (_Instance == null)
        {
            _Instance = (T)this;
            SceneManager.sceneLoaded += OnSceneLoaded;
            DontDestroyOnLoad(gameObject);
        }
        else if (_Instance != this)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) { }
}
