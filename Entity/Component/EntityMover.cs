using Hashira.Core.MoveSystem;
using Hashira.Players;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

namespace Hashira.Entities
{
    public class EntityMover : MonoBehaviour, IEntityComponent
    {
        [field: SerializeField] public Collider2D Collider2D { get; private set; }
        [field: SerializeField] public Rigidbody2D Rigidbody2D { get; private set; }

        [Header("Ground check setting")]
        [SerializeField] protected Vector2 _checkerSize;
        [SerializeField] protected float _downDistance;
        [SerializeField] protected float _upDistance;
        [SerializeField] protected LayerMask _whatIsGround;
        [SerializeField] private LayerMask _oneWayPlatform;

        private float _maxSlopeAngle = 45f;

        protected RaycastHit2D _hitedGround;
        protected bool _gravityActive;
        protected float _gravity;
        protected float _gravityScale;

        protected Entity _entity;

        protected float _yMovement;
        protected Vector2 _movement;
        public Vector2 ToMove { get; private set; }
        public Vector2 Velocity { get; private set; }

        public bool IsGrounded => _hitedGround != default && _yMovement < 0;
        public bool IsOneWayPlatform { get; private set; }
        public bool isManualMove = true;

        [field: SerializeField] public int JumpCount { get; private set; } = 1;
        private int _currentJumpCount = 0;
        [field: SerializeField] public float JumpPower { get; private set; }


        private Dictionary<Type, MoveProcessor> _moveProcessorDict;


        public virtual void Initialize(Entity entity)
        {
            _moveProcessorDict = new Dictionary<Type, MoveProcessor>();

            _gravity = Physics2D.gravity.y;
            _gravityScale = Rigidbody2D.gravityScale;
            Rigidbody2D.gravityScale = 0;
            _entity = entity;
            isManualMove = true;
            _gravityActive = true;
            _whatIsGround |= _oneWayPlatform;

            AddMoveProcessor<ApplyVelocityProcessor>();
        }

        protected virtual void FixedUpdate()
        {
            GroundAndCeilingCheck();
            ApplyGravity();
            ApplyProcessor();
            ApplyVelocity();
        }

        private void ApplyProcessor()
        {
            Vector3 movement;
            if (_gravityActive)
            {
                movement = Vector3.ProjectOnPlane(_movement, transform.up).normalized * _movement.magnitude;
            }
            else
                movement = _movement;
            //if (_entity is Player) Debug.Log($"{transform.up} dot {_hitedGround.normal}");
            //if (_entity is Player) Debug.Log($"{Mathf.Acos(Vector3.Dot(transform.up, _hitedGround.normal)) * Mathf.Rad2Deg} >= {_maxSlopeAngle}");
            if (IsGrounded)
            {
                ToMove = Vector3.ProjectOnPlane(movement, _hitedGround.normal).normalized * movement.magnitude + (Vector3)_hitedGround.normal * _yMovement;
            }
            else
            {
                ToMove = Vector3.ProjectOnPlane(movement, transform.up).normalized * 
                    movement.magnitude + transform.up * _yMovement;
            }
            foreach (var processor in _moveProcessorDict.Values)
            {
                if (processor.IsActive)
                    Velocity = processor.ProcessMove(Velocity);
            }
        }

        private void GroundAndCeilingCheck()
        {
            RaycastHit2D[] groundHits = Physics2D.BoxCastAll((Vector2)transform.position,
                _checkerSize, _entity.transform.eulerAngles.z, -_entity.transform.up, _downDistance, _whatIsGround);

            if (groundHits.Length > 0 && Vector3.Dot(transform.up, groundHits[0].normal) >= Mathf.Cos(_maxSlopeAngle * Mathf.Deg2Rad))
            {
                _hitedGround = groundHits[0];
            }
            else
            {
                _hitedGround = default;
            }

            if (groundHits.Length > 0) IsOneWayPlatform = groundHits[0].transform.gameObject.GetComponent<PlatformEffector2D>();

            if (IsGrounded) _currentJumpCount = 0;

            RaycastHit2D[] ceilingHits = Physics2D.BoxCastAll((Vector2)transform.position,
                _checkerSize, _entity.transform.eulerAngles.z, _entity.transform.up, _upDistance, _whatIsGround);

            if (ceilingHits.Length > 0 && _yMovement > 0) _yMovement = 0;
        }


        private void ApplyVelocity()
        {
            if (Rigidbody2D.bodyType == RigidbodyType2D.Static) return;
            Rigidbody2D.linearVelocity = Velocity;
        }

        public void SetGravity(bool active)
        {
            _gravityActive = active;
        }

        private void ApplyGravity()
        {
            if (_gravityActive == false) return;

            if (IsGrounded)
            {
                _yMovement = -3;
            }
            else
            {
                _yMovement += _gravity * Time.fixedDeltaTime * _gravityScale;
            }
        }

        public void SetMovement(Vector2 movement, bool isForcedMove = false)
        {
            if (isManualMove == false && isForcedMove == false) return;
            _movement = movement;
        }

        public void Jump()
        {
            if (isManualMove == false) return;
            if (_currentJumpCount >= JumpCount)
                return;

            _yMovement = JumpPower;

            ++_currentJumpCount;
        }

        public void StopImmediately(bool withYVelocity = false)
        {
            if (isManualMove == false) return;
            _movement = Vector3.zero;
            //Rigidbody2D.linearVelocityX = 0;

            if (withYVelocity)
            {
                _yMovement = 0;
                //Rigidbody2D.linearVelocityY = 0;
            }
        }

        public void UnderJump(bool isUnderJump)
        {
            if (IsOneWayPlatform == false) return;
            if (isManualMove == false) return;

            int layerCheck = _whatIsGround & _oneWayPlatform;
            if (layerCheck != 0 && isUnderJump)
                _whatIsGround &= ~(_oneWayPlatform);
            else if (isUnderJump == false)
                _whatIsGround |= _oneWayPlatform;

            Collider2D.isTrigger = isUnderJump;
        }

        public void SetIgnoreOnewayPlatform(bool isIgnore)
        {
            if (isIgnore)
                _whatIsGround &= ~(_oneWayPlatform);
            else
                _whatIsGround |= _oneWayPlatform;
        }

        public void AddMoveProcessor<T>() where T : MoveProcessor, new()
        {
            T moveProcessor = new T();
            _moveProcessorDict.Add(typeof(T), moveProcessor);
            moveProcessor.Initialize(this);
        }

        public T GetMoveProcessor<T>() where T : MoveProcessor
        {
            if (_moveProcessorDict.TryGetValue(typeof(T), out MoveProcessor processor))
                return processor as T;
            return default;
        }

        public void RemoveMoveProcessor<T>() where T : MoveProcessor
        {
            Type type = typeof(T);
            if (_moveProcessorDict.ContainsKey(type))
            {
                _moveProcessorDict.Remove(type);
            }
        }

        public T SetActiveMoveProcessor<T>(bool isActive) where T : MoveProcessor
        {
            Type type = typeof(T);
            if (_moveProcessorDict.TryGetValue(type, out var processor))
            {
                processor.IsActive = isActive;
                return processor as T;
            }
            return default;
        }


#if UNITY_EDITOR
        protected virtual void OnDrawGizmos()
        {
            if (_entity != null)
                Gizmos.matrix = Matrix4x4.TRS(transform.position, _entity.transform.rotation, Vector3.one);
            else
                Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.parent.rotation, Vector3.one);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(Vector3.zero, _checkerSize);
            Gizmos.color = Color.blue;
            Vector3 target1Pos = Vector3.down * _downDistance;
            Gizmos.DrawLine(Vector3.zero, target1Pos);
            Gizmos.DrawWireCube(target1Pos, _checkerSize);
            Gizmos.color = Color.blue;
            Vector3 target2Pos = Vector3.up * _upDistance;
            Gizmos.DrawLine(Vector3.zero, target2Pos);
            Gizmos.DrawWireCube(target2Pos, _checkerSize);
        }
#endif
    }
}