using Hashira;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UIManager = Hashira.CanvasUI.UIManager;

public class SceneLoadingManager : MonoBehaviour
{
    private static string _nextScene;
    public static event Action OnSceneLoadStartEvent;
    public static event Action OnSceneLoadCompleteEvent;
    
    private static SceneLoadingManager _instance;

    private void Awake()
    {
        URPScreenEffect.Fade(true, 1f);
        _instance = this;
    }

    public static void LoadScene(string sceneName)
    {
        // 다른 UI 전부 비활성화
        UIManager.UIInteractor.Interactable = false;
        
        URPScreenEffect.Fade(false, 1f);
        _nextScene = sceneName;
        OnSceneLoadStartEvent?.Invoke();
        _instance.LoadSceneProcess();
    }

    private async void LoadSceneProcess()
    {
        var op = SceneManager.LoadSceneAsync(_nextScene);
        op.allowSceneActivation = false;
        
        await WaitUntilComplete(op);
        await Task.Delay(500);
        OnLoadSceneComplete(op);
    }

    private async Task WaitUntilComplete(AsyncOperation op)
    {
        while (op.progress < 0.9f)
        {
            await Task.Delay(100);
        }
    }

    private async void OnLoadSceneComplete(AsyncOperation op)
    {
        OnSceneLoadCompleteEvent?.Invoke();
        await Task.Delay(200);
        op.allowSceneActivation = true;
    }
}
