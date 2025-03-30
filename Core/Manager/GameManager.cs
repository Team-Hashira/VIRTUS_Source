using DG.Tweening;
using Hashira.CanvasUI;
using Hashira.Cards;
using Hashira.Entities;
using Hashira.MainScreen;
using Hashira.Players;
using Hashira.StageSystem;
using UnityEngine;

namespace Hashira.Core
{
    public class GameManager : MonoSingleton<GameManager>
    {
        [SerializeField] private StageGenerator _stageGenerator;
        [SerializeField] private InputReaderSO _inputReader;

        private EntityMover _playerMover;
        private ToggleDomain _toggleDomain;

        private Sequence _gameOverSeq;
        public bool IsGameOver { get; private set; }
        
        private void Awake()
        {
            _stageGenerator.GenerateStage();
            IsGameOver = false;
        }

        private void Start()
        {
            _toggleDomain = Hashira.CanvasUI.UIManager.Instance.GetDomain<ToggleDomain>();
            _playerMover = PlayerManager.Instance.Player.GetEntityComponent<EntityMover>(true);
        }

        public void ClearStage()
        {
            _inputReader.PlayerActive(false);
            Player player = PlayerManager.Instance.Player;
            player.transform.position = new Vector3(0, 10000, 0);
            EntityHealth entityHealth = player.GetEntityComponent<EntityHealth>();
            PlayerDataManager.Instance.SetHealth(entityHealth.Health, entityHealth.MaxHealth);
        }

        public void GameOver()
        {
            if (IsGameOver) return;


            IsGameOver = true;
            _gameOverSeq = DOTween.Sequence();
            _gameOverSeq
                .AppendCallback(() =>
                {
                    MainScreenEffect.OnGlitch(1, 1.25f);
                    _inputReader.PlayerActive(false);
                })
                .AppendInterval(1f)
                .AppendCallback(() =>
                {
                    MainScreenEffect.OnScaling(0);
                })
                .AppendInterval(0.25f)
                .AppendCallback(() =>
                {
                    Destroy(StageGenerator.Instance.GetCurrentStage());
                })
                .AppendInterval(0.25f)
                .AppendCallback(() =>
                {
                    IToggleUI toggleUI = Hashira.CanvasUI.UIManager.Instance.GetDomain<ToggleDomain>().OpenUI("GameEndStatistics");
                    GameEndStatistics gameEndStatistics = toggleUI as GameEndStatistics;
                    gameEndStatistics.Init(StageGenerator.currentFloorIdx + 1, StageGenerator.currentStageIdx + 1, PlayerDataManager.Instance.KillCount, PlayerDataManager.Instance.BossKillCount, 
                        PlayerDataManager.Instance.CardEffectList);

                    Hashira.CanvasUI.UIManager.Instance.GetDomain<ToggleDomain>().CloseUI("PlayerDataUIPanel");
                    PlayerDataManager.Instance.ResetData();
                });
        }
    }
}
