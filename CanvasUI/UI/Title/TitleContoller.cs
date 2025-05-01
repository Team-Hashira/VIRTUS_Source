using Hashira.CanvasUI.Option;
using UnityEngine;

namespace Hashira.CanvasUI.Title
{
    public class TitleContoller : MonoBehaviour
    {
        [SerializeField] private CustomButton _gameBtn, _tutorialBtn, _optionBtn, _gameQuitBtn;

        private void Awake()
        {
            _gameBtn.OnClickEvent += () => SceneLoadingManager.LoadScene(SceneName.CardSelectScene);
            _tutorialBtn.OnClickEvent += () => SceneLoadingManager.LoadScene(SceneName.TutorialScene);
            _optionBtn.OnClickEvent += () => UIManager.Instance.GetDomain<ToggleDomain>().OpenUI("Option");
            _gameQuitBtn.OnClickEvent += () => Application.Quit();
        }
    }
}
