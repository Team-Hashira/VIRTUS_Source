using DG.Tweening;
using Hashira.Core;
using Hashira.Players;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hashira.Bosses
{
    public class GiantGolemEye : MonoBehaviour
    {
        private Player _player;
        [SerializeField] private Transform _originTransform;
        [SerializeField] private float _maxDistance = 0.1f;
        [SerializeField] private float _LookAtPlayerStartDelay = 1f;
        [SerializeField] private bool _lookAtPlayerDirection = false;

        public bool LookAtPlayerDirection
        {
            get=> _lookAtPlayerDirection;
            set
            {
                _lookAtPlayerDirection = value;
                if (_lookAtPlayerDirection == false)
                {
                    transform.DOLocalMove(Vector3.zero, 0.1f);
                }
            }
        }
        
        private void Awake()
        {
            _player = PlayerManager.Instance.Player;
        }

        private void Start()
        {
            StartCoroutine(CoroutineLookAtPlayerDirectionStart());
        }

        private void Update()
        {
            if (_lookAtPlayerDirection == false) return;
            Vector2 direction = _player.transform.position - _originTransform.position;
            direction = direction.normalized * _maxDistance;
            transform.DOLocalMove(direction / transform.lossyScale, 0.1f);
        }

        private IEnumerator CoroutineLookAtPlayerDirectionStart()
        {
            yield return new WaitForSeconds(_LookAtPlayerStartDelay);
            _lookAtPlayerDirection = true;
        }
    }
}
