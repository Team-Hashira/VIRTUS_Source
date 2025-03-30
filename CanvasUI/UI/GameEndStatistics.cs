using Hashira.CanvasUI.Option;
using Hashira.CanvasUI.Title;
using Hashira.Cards;
using Hashira.Cards.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Hashira.CanvasUI
{
    public class GameEndStatistics : UIBase, IToggleUI
    {
        [field: SerializeField] public string Key { get; set; }

        [SerializeField] private TextMeshProUGUI _statisticsText;
        [SerializeField] private CardScrollView _cardScrollView;
        [SerializeField] private CustomButton _titleBtn;
        [SerializeField] private InputReaderSO _inputReader;

        protected override void Awake()
        {
            base.Awake();
            _titleBtn.OnClickEvent += HandleTitleEvent;
        }

        private void Start()
        {
            Close();
        }

        private void HandleTitleEvent()
        {
            _inputReader.PlayerActive(true);
            SceneLoadingManager.LoadScene(SceneName.TitleScene);
        }

        public void Init(int floor, int stage, int killCound, int bossKillCound, List<CardEffect> cardEffectList)
        {
            _cardScrollView.CreateCard(cardEffectList.Select(cardEffect => cardEffect.CardSO).ToList());
            int score = (floor - 1) * 20 + (stage - 1) * 3 + killCound * 2 + bossKillCound * 10;
            _statisticsText.text =  $"진척도 : {floor}층 {stage}번 구역\n" +
                                    $"잡은 유해생명체 : {killCound}체\n" +
                                    $"잡은 유해근원체 : {bossKillCound}체\n" +
                                    $"특수 구역 정보수집 : 0회\n" +
                                    $"-------------------------\n" +
                                    $"보수 : {score}";
            Score.AddScore(score);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            _titleBtn.OnClickEvent -= HandleTitleEvent;
        }
    }
}
