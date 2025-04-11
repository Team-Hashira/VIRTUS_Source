using DG.Tweening;
using Hashira.Bosses;
using Hashira.StageSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hashira.UI
{
    public class BossHealthUI : MonoBehaviour
    {
        [SerializeField] private BossHealthBar _bossHealthBarPrefab;
        private List<BossHealthBar> _currentBossHealthBars;
        private RectTransform _rectTransform;

        private int _currentBossCount = 0;
        
        private void Awake()
        {
            _currentBossHealthBars = new List<BossHealthBar>();
            _rectTransform = transform as RectTransform;
        }

        private void Start()
        {
            _rectTransform.anchoredPosition = new Vector2(0, 145);
            StageGenerator.Instance.OnGeneratedStageEvent += OnCheckBossStageHandle;
        }

        private void OnDestroy()
        {
            StageGenerator.Instance.OnGeneratedStageEvent -= OnCheckBossStageHandle;
        }

        private void OnCheckBossStageHandle()
        {
            if (StageGenerator.Instance.IsCurrentBossStage() == false)
            {
                _rectTransform.anchoredPosition = new Vector2(0, 145);
            }
        }

        private void Update()
        {
            if (Boss.CurrentBosses == null || Boss.CurrentBosses.Count == _currentBossCount) return;

            _currentBossCount = Boss.CurrentBosses.Count;
            CreateBossHealthBars();
        }

        private void CreateBossHealthBars()
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(_rectTransform.DOAnchorPosY(145, 1f));
            seq.AppendCallback(() =>
            {
                foreach (var bossHealthBar in _currentBossHealthBars.Where(bossHealthBar => Boss.CurrentBosses.ContainsKey(bossHealthBar.Boss.BossDisplayName) == false).ToList())
                    Destroy(bossHealthBar.gameObject);

                _currentBossHealthBars = _currentBossHealthBars.Where(bossHealthBar => bossHealthBar !=null && Boss.CurrentBosses.ContainsKey(bossHealthBar.Boss.BossDisplayName)).ToList();

                // HealthBar 생성
                foreach (var currentBoss in Boss.CurrentBosses)
                {
                    bool isContained = _currentBossHealthBars.Any(bossHealthBar => currentBoss.Value == bossHealthBar.Boss);

                    if (isContained == false)
                    {
                        var bossHealthBar = Instantiate(_bossHealthBarPrefab, transform);
                        _currentBossHealthBars.Add(bossHealthBar);
                        bossHealthBar.Init(currentBoss.Value);
                    }
                }

                for (int i = 0; i < _currentBossHealthBars.Count; i++)
                {
                    for (int j = 0; j < _currentBossHealthBars.Count; j++)
                    {
                        if (_currentBossHealthBars[j].Boss.Priority == i)
                        {
                            _currentBossHealthBars[j].transform.SetAsLastSibling();
                            break;
                        }
                    } 
                }   
            });
            seq.Append(_rectTransform.DOAnchorPosY(0, 1f));
        }
    }
}
