using DG.Tweening;
using Hashira.Bosses.Patterns.GiantGolem;
using Hashira.Entities.Components;
using Hashira.Players;
using System;
using System.Linq;
using UnityEngine;

namespace Hashira.Bosses.Patterns
{
    public class GiantGolemHand : Boss
    {
        private IGiantGolemHandEffect[] _handEffects;
        private IGiantGolemHandEffect _currentActiveEffect;
        private bool _currentActive = false;
        public Vector3 OriginPosition { get; private set; }
        public Quaternion OriginRotation {get; private set;}
        private int _handDirection;
        public int HandDirection
        {
            get => _handDirection;
            set
            {
                _handDirection = value;
                _animator.SetFloat(_directionHash, _handDirection);
            }
        }
        public Vector2 HandNormalizedDirection
        {
            get => new Vector2(1 - Mathf.Abs(_handDirection), _handDirection);
            set => HandDirection = (int)value.y;
        }

        private GiantGolemHandAnimator _animator;

        private readonly int _directionHash = Animator.StringToHash("Direction");
        private readonly int _attackHash = Animator.StringToHash("Attack");

        
        [field:SerializeField]public Transform VisualTransform { get; private set; }
        
        private void Awake()
        {
            _animator = GetComponentInChildren<GiantGolemHandAnimator>();
            _animator.OnAnimationTriggeredEvent += CurrentAnimationTrigger;
            OriginPosition = transform.position;
            _handEffects = GetComponentsInChildren<IGiantGolemHandEffect>();
            for (int i = 0; i < _handEffects.Length; i++)
            {
                _handEffects[i].Init(this);
            }
            
            OriginRotation = transform.rotation;
        }

        private void OnDestroy()
        {
            _animator.OnAnimationTriggeredEvent -= CurrentAnimationTrigger;
        }

        public T GetHandEffect<T>() where T : class, IGiantGolemHandEffect
        {
            return _handEffects.FirstOrDefault(x => x.GetType() == typeof(T)) as T;
        }
        
        public T SetActiveEffect<T>(bool active) where T : class, IGiantGolemHandEffect
        {
            _currentActive = active;
            IGiantGolemHandEffect effect = _handEffects.FirstOrDefault(x => x.GetType() == typeof(T));
            if (active)
            {
                _animator.OnAnimationTriggeredEvent += effect.OnAnimatorTrigger;
                _currentActiveEffect = effect;
            }
            effect?.SetActive(active);
            _animator.SetBool(_attackHash, active);
            return effect as T;
        }
        public T SetActiveEffect<T>(bool active, Vector2 dir) where T : class, IGiantGolemHandEffect
        {
            HandNormalizedDirection = dir;
            return SetActiveEffect<T>(active);
        }

        public void SetActiveEffect(bool active)
        {
            _animator.SetBool(_attackHash, active);
        }
        public void SetActiveEffect(bool active, Vector2 dir)
        {
            SetActiveEffect(active);
            HandNormalizedDirection = dir;
        }

        public Tween ResetToOriginPosition(float duration = 0.25f)
        {
            return transform.DOMove(OriginPosition, duration);
        }

        public void Translate(Vector2 movement)
        {
            transform.DORotate(new Vector3(0, 0, MathExtension.Sign(movement.x) * 3f), 0.1f);
            transform.Translate(movement, Space.World);
        }

        public void MoveToPlayer(Player player, float maxSpeed, bool moveX = true, bool moveY = true)
        {
            if (moveX)
            {
                float dir = player.transform.position.x - transform.position.x;
                if (Mathf.Abs(dir) < 0.35f) return;
                float normalizedDir = MathExtension.Sign(dir);
                Translate(new Vector3(normalizedDir * maxSpeed * Time.deltaTime, 0));    
            }

            if (moveY)
            {
                float dir = player.transform.position.y - transform.position.y;
                if (Mathf.Abs(dir) < 0.35f) return;
                float normalizedDir = MathExtension.Sign(dir);
                Translate(new Vector3(0, Mathf.Lerp(0, maxSpeed * normalizedDir, Time.deltaTime)));
            }
        }

        private void CurrentAnimationTrigger(EAnimationTriggerType triggerType , int count)
        {
            if (_currentActiveEffect != null && triggerType == EAnimationTriggerType.End && _currentActive == false)
            {
                _animator.OnAnimationTriggeredEvent -= _currentActiveEffect.OnAnimatorTrigger;
                _currentActiveEffect = null;
            }
        }
    }
}
