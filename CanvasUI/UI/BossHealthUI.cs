using DG.Tweening;
using Hashira.Bosses;
using Hashira.Core;
using Hashira.Entities;
using Hashira.Players;
using Hashira.StageSystem;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.UI
{
    public class BossHealthUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _bossNameText;
        [SerializeField] private Sprite[] _backgroundSprites;
        [SerializeField] private BossHealthBar _bossHealthBarPrefab;
        private List<BossHealthBar> _currentBossHealthBars;
        
        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;
        private Image _image;
        
        private int _currentBossCount = 0;
        
        private Player _player;
        private StageGenerator _stageGenerator;
        
        private void Awake()
        {
            _currentBossHealthBars = new List<BossHealthBar>();
            _rectTransform = transform as RectTransform;
            _canvasGroup = GetComponent<CanvasGroup>();
            _image = GetComponent<Image>();
        }

        private void Start()
        {
            _player = PlayerManager.Instance.Player;
            _stageGenerator = StageGenerator.Instance;
            
            _rectTransform.anchoredPosition = new Vector2(0, 145);
            _player.EntityHealth.OnDieEvent += OnPlayerDieHandle;
            _stageGenerator.OnGeneratedStageEvent += OnCheckBossStageHandle;
        }

        private void OnDestroy()
        {
            _stageGenerator.OnGeneratedStageEvent -= OnCheckBossStageHandle;
            _player.EntityHealth.OnDieEvent -= OnPlayerDieHandle;
            _rectTransform?.DOKill(true);
        }

        private void OnPlayerDieHandle(Entity _)
        {
            _canvasGroup.DOFade(0, 0.5f).OnComplete(()=> _rectTransform.anchoredPosition = new Vector2(0, 145));
        }
        
        private void OnCheckBossStageHandle()
        {
            _canvasGroup.alpha = 1;
            if (_stageGenerator.IsCurrentBossStage() == false)
            {
                _rectTransform.anchoredPosition = new Vector2(0, 145);
            }
        }

        private void Update()
        {
            if (Boss.CurrentBosses == null || Boss.CurrentBosses.Count == _currentBossCount) return;

            // 나중에 무적인 애들은 Count에서 자체적으로 빼야 함
            _currentBossCount = Boss.CurrentBosses.Count;
            CreateBossHealthBars();
        }
        
        private void CreateBossHealthBars()
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(_rectTransform.DOAnchorPosY(145, 1f));
            seq.AppendCallback(() =>
            {
                // 보스가 죽거나 무적이라면 HealthBar를 파괴
                foreach (var bossHealthBar in _currentBossHealthBars.Where(bossHealthBar => bossHealthBar.Boss ==null || bossHealthBar.Boss.EntityHealth.GetEvasion() > 0).ToList())
                    Destroy(bossHealthBar.gameObject);

                // 현재 남아있는 HealthBar를 체크
                _currentBossHealthBars = _currentBossHealthBars.Where(bossHealthBar => bossHealthBar !=null && bossHealthBar.Boss != null).ToList();

                // 현재 보스들을 Priority로 정렬한 상태로 저장(무적 상태인 애들은 제외)
                var sortedCurrentBosses = Boss.CurrentBosses
                    .Where(x => x.Value.EntityHealth.GetEvasion() <= 0) // 나중에 이것도 지워야 함(무적이 아닌 애들)
                    .OrderBy(x => x.Value.Priority);

                foreach (var currentBoss in sortedCurrentBosses)
                {
                    // 이미 Boss가 지정 되어있는 HealthBar는 생성하지 않기 위해 체크
                    bool isContained = _currentBossHealthBars.Any(bossHealthBar => currentBoss.Value == bossHealthBar.Boss);
                    
                    // HealthBar가 없다면? HealthBar 생성
                    if (isContained == false)
                    {
                        var bossHealthBar = Instantiate(_bossHealthBarPrefab, transform);
                        _currentBossHealthBars.Add(bossHealthBar);
                        bossHealthBar.Init(currentBoss.Value);
                        bossHealthBar.transform.SetAsLastSibling();
                    }
                }
                
                // 보스의 수에 따라 배경의 이미지를 다르게
                _image.sprite = _backgroundSprites[sortedCurrentBosses.Count()-1];
                
                // 서브 보스가 아닌 보스의 이름을 가지고 오기
                _bossNameText.text = Boss.CurrentBosses.FirstOrDefault(x => x.Value.IsPassive == false).Key;
            });
            if (_currentBossCount != 0) seq.Append(_rectTransform.DOAnchorPosY(0, 1f));
        }
    }
}
