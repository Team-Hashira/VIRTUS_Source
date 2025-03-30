using DG.Tweening;
using Hashira.Core;
using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.LightingControl;
using Hashira.MainScreen;
using System;
using System.Collections;
using UnityEngine;

namespace Hashira.Players
{
    public class Player : Entity
    {
        [field: SerializeField] public InputReaderSO InputReader { get; private set; }
        [field: SerializeField] public Transform VisualTrm { get; private set; }
        [field: SerializeField] public ParticleSystem AfterImageParticle { get; private set; }

        public Attacker Attacker { get; private set; }

        protected EntityStateMachine _stateMachine;
        protected EntityRenderer _renderCompo;
        protected EntityStat _statCompo;
        protected EntityInteractor _interactor;
        protected PlayerMover _playerMover;

        protected StatElement _damageStat;

        [Header("=====Stamina setting=====")]
        [field: SerializeField] public int MaxStamina { get; private set; }
        [SerializeField] private float _staminaRecoveryDelay;
        private float _lastStaminaChangedTime;
        private int _currentStamina;
        public event Action<int, int> OnStaminaChangedEvent;

        [Header("=====Layer setting=====")]
        [SerializeField] private LayerMask _whatIsTarget;
        [SerializeField] private LayerMask _whatIsObstacle;

        private float _slowTimeScale = 0.1f;
        private float _chargingParryingStartDelay = 0.15f;
        private float _lastRightClickTime;
        private bool _isRightMousePress;
        private bool _isChargingParrying;

        private EntityHealth _entityHealth;

        [Header("=====Evasion Setting=====")]
        [SerializeField]
        private float _evasionTime = 1f;

        protected override void Awake()
        {
            base.Awake();

            _entityHealth = GetEntityComponent<EntityHealth>();

            int prevStageHealth = PlayerDataManager.Instance.Health;
            if (prevStageHealth != -1) _entityHealth.SetHealth(prevStageHealth);

            _entityHealth.OnHealthChangedEvent += HandleHealthChange;
            InputReader.OnDashEvent += HandleDashEvent;
            InputReader.OnInteractEvent += HandleInteractEvent;
            InputReader.OnSprintToggleEvent += HandleSprintToggle;

            //InputReader.OnReloadEvent += _weaponGunHolderCompo.Reload; //재장전 만들꺼면 다시 구현
            InputReader.OnAttackEvent += HandleAttackEvent;
        }

        private void Start()
        {
            PlayerManager.Instance.SetCardEffectList(PlayerDataManager.Instance.CardEffectList, true);
            _currentStamina = MaxStamina;
        }

        private void OnDisable()
        {
            //if (TargetPointManager.Instance != null)
            //    TargetPointManager.Instance.CloseTargetPoint(transform);
        }

        #region Handles

        private void HandleHealthChange(int old, int cur)
        {
            if (old > cur)
            {
                SoundManager.Instance.PlaySFX("PlayerHit", transform.position, 1f);
                CameraManager.Instance.ShakeCamera(5, 5, 0.3f);
                StartCoroutine(EvasionCoroutine());
                MainScreenEffect.OnGlitch(0.2f, 0).OnComplete(() => MainScreenEffect.OnGlitch(0, 0.5f));
                LightingController.Aberration(1f, 0.3f);
                StartCoroutine(TimeCoroutine());
            }

            if (cur <= 0)
            {
                GameManager.Instance.GameOver();
            }
        }

        private IEnumerator TimeCoroutine()
        {
            float percent = 0.1f;
            while (percent < 1)
            {
                percent += Time.deltaTime * 5f;
                TimeController.SetTimeScale(percent);
                yield return null;
            }
            TimeController.ResetTimeScale();
        }

        private IEnumerator EvasionCoroutine()
        {
            _entityHealth.ModifyEvasion(true);
            yield return new WaitForSeconds(_evasionTime);
            _entityHealth.ModifyEvasion(false);
        }

        private void HandleInteractEvent(bool isDown)
        {
            _interactor.Interact(isDown);
        }

        private void HandleSprintToggle()
        {
            _playerMover.OnSprintToggle();
            _stateMachine.ChangeState(_playerMover.IsSprint ? "Run" : "Walk");
        }

        private void HandleDashEvent()
        {
            if (_playerMover.CanRolling == false) return;
            if (_stateMachine.CurrentStateName != "Rolling")
            {
                if (TryUseStamina(1))
                {
                    _playerMover.OnDash();
                    _stateMachine.ChangeState("Rolling");
                }
                else
                {
                    Debug.Log("스테미나 부족!");
                }
            }
        }

        private void HandleAttackEvent(bool isDown)
        {
            //공격
        }

        #endregion

        protected override void InitializeComponent()
        {
            Attacker = FindAnyObjectByType<Attacker>();
            base.InitializeComponent();

            _playerMover = GetEntityComponent<PlayerMover>();
            _statCompo = GetEntityComponent<EntityStat>();
            _renderCompo = GetEntityComponent<EntityRenderer>();
            _interactor = GetEntityComponent<EntityInteractor>();
            _stateMachine = GetEntityComponent<EntityStateMachine>();
            _damageStat = _statCompo.StatDictionary[StatName.AttackPower];
        }

        protected override void AfterIntiialize()
        {
            base.AfterIntiialize();
        }

        public bool TryUseStamina(int usedValue)
        {
            if (_currentStamina < usedValue) return false;
            int prevStamina = _currentStamina;
            _currentStamina -= usedValue;
            _lastStaminaChangedTime = Time.time;
            OnStaminaChangedEvent?.Invoke(prevStamina, _currentStamina);
            return true;
        }

        protected override void Update()
        {
            base.Update();

            if (_currentStamina < MaxStamina && _lastStaminaChangedTime + _staminaRecoveryDelay < Time.time)
            {
                _lastStaminaChangedTime = Time.time;
                int prevStamina = _currentStamina;
                _currentStamina++;
                OnStaminaChangedEvent?.Invoke(prevStamina, _currentStamina);
            }


            if (_isRightMousePress && _isChargingParrying == false &&
                _lastRightClickTime + _chargingParryingStartDelay < Time.time)
            {
                _isChargingParrying = true;
                TimeController.SetTimeScale(_slowTimeScale);
            }
            if (_isChargingParrying)
            {
                //차징중...
            }
            
            // 데미지업 디버그
//#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.O))
            {
                _damageStat.AddModify("DebugDamageUp", 100, EModifyMode.Add, EModifyLayer.Default);
            }
//#endif
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _entityHealth.OnHealthChangedEvent -= HandleHealthChange;
            InputReader.OnDashEvent -= HandleDashEvent;
            InputReader.OnInteractEvent -= HandleInteractEvent;
            InputReader.OnSprintToggleEvent -= HandleSprintToggle;

            //InputReader.OnReloadEvent -= _weaponGunHolderCompo.Reload; //재장전시 구현
            InputReader.OnAttackEvent -= HandleAttackEvent;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            _playerMover.OnCollision(collision);
        }
    }
}