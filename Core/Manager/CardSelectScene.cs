using Hashira.Cards;
using Hashira.CanvasUI;
using UnityEngine;
using Hashira.StageSystem;

namespace Hashira
{
    public class CardSelectScene : MonoSingleton<CardSelectScene>
    {
        [SerializeField] private InputReaderSO _inputReader;

        private ToggleDomain _toggleDomain;

        private void Start()
        {
            _toggleDomain = Hashira.CanvasUI.UIManager.Instance.GetDomain<ToggleDomain>();
            _inputReader.PlayerActive(false);
            StartCardUse();

            if (PlayerDataManager.Instance.Health > PlayerDataManager.Instance.MaxHealth)
                PlayerDataManager.Instance.SetHealth(PlayerDataManager.Instance.MaxHealth, PlayerDataManager.Instance.MaxHealth);
        }

        public void StartCardUse()
        {
            _toggleDomain.OpenUI("CardUsingUI");
            _toggleDomain.OpenUI("AppliedCardPanel");
        }
        //public void StartCardSelect()
        //{
        //    _toggleDomain.OpenUI("RewardCardPanel");
        //}

        public void StartStage()
        {
            _toggleDomain.CloseUI("AppliedCardPanel");
            _inputReader.PlayerActive(true);
            StageGenerator.currentStageIdx++;
            SceneLoadingManager.LoadScene(SceneName.GameScene);
        }
    }
}
