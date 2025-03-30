using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingManager : MonoBehaviour
{
    private static string _nextScene;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private TextMeshProUGUI _tipText;
    [SerializeField] private string[] _tips;
    private int _currentTip;

    [SerializeField] private float _dotDelay = 0.1f;
    private float _dotTime = 0f;
    private int _dotCount = 0;

    public static void LoadScene(string sceneName)
    {
        _nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    public async static void LoadScene(string sceneName, float delay)
    {
        await Task.Delay((int)(delay * 1000));
        _nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    private void Start()
    {
        _currentTip = Random.Range(0, _tips.Length);
        SetTipText();
        LoadSceneProcess();
    }

    private void Update()
    {
        if (_dotDelay + _dotTime < Time.time)
        {
            _descriptionText.text = $"Download";

            _dotCount = (_dotCount + 1) % 4;

            for (int i = 0; i < _dotCount; i++)
                _descriptionText.text += ".";

            _dotTime = Time.time;
        }


        if (Input.GetMouseButtonDown(0))
        {
            _currentTip = (_currentTip + 1) % _tips.Length;
            SetTipText();
        }
    }

    private void SetTipText()
    {
        _tipText.text = _tips[_currentTip];
    }

    private async void LoadSceneProcess()
    {
        var op = SceneManager.LoadSceneAsync(_nextScene);
        op.allowSceneActivation = false;

        _descriptionText.text = string.Empty;

        //for (int i = 0; i < 3; i++)
        //{
        //    for (int j = 0; j < 3; j++)
        //    {
        //        await Task.Delay(250);
        //    }
        //}

        await WaitUntilComplete(op);

        LoadComplete(op);
    }

    private async Task WaitUntilComplete(AsyncOperation op)
    {
        while (op.progress < 0.9f)
        {
            await Task.Delay(100);
        }
    }

    private void LoadComplete(AsyncOperation op)
    {
        OnLoadSceneComplete(op);
    }

    private async void OnLoadSceneComplete(AsyncOperation op)
    {
        await Task.Delay(200);

        op.allowSceneActivation = true;
    }
}
