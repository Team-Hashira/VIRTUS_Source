using System;
using UnityEngine;
using UnityEngine.Events;

namespace Hashira.Tutorials
{
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public class CollisionCheckStep : TutorialStep
    {
        [SerializeField]
        private string _targetTag;
        private Collider2D _collider;

        public UnityEvent OnCollisionEvent;

        public override void Initialize(TutorialManager tutorialManager)
        {
            base.Initialize(tutorialManager);
            _collider = GetComponent<Collider2D>();
            _collider.enabled = false;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _collider.enabled = true;
        }

        public override void OnExit()
        {
            base.OnExit();
            _collider.enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag(_targetTag))
                OnCollisionEvent?.Invoke();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag(_targetTag))
                OnCollisionEvent?.Invoke();
        }
    }
}
