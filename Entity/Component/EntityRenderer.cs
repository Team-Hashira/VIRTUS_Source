using DG.Tweening;
using Hashira.Core;
using Hashira.MainScreen;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hashira.Entities
{
    public delegate void OnFlipEvent(float facingDirection);

    public class EntityRenderer : MonoBehaviour, IEntityComponent
    {
        private readonly static int _BlinkShanderkHash = Shader.PropertyToID("_Blink");
        private readonly static int _DissolveShader = Shader.PropertyToID("_Dissolve");

        private Entity _entity;

        [field: SerializeField] public Transform VisualTrm { get; private set; }
        [field: SerializeField] public Transform RotationTrm { get; private set; }
        //[SerializeField] private List<GameObject> _armObject;
        [SerializeField] private bool _onFlip;

        public List<SpriteRenderer> SpriteRendererList { get; private set; }
        public float FacingDirection { get; private set; }
        public Vector3 UsualFacingTarget { get; private set; }
        public bool isUsualFacing = true;

        private List<Tween> _blinkTweenList;
        private List<Tween> _dissolveTweenList;

        public event OnFlipEvent OnFlipEvent;

        public void Initialize(Entity entity)
        {
            _entity = entity;

            _blinkTweenList = new List<Tween>();
            FacingDirection = 1;
            isUsualFacing = false;
            SpriteRendererList = GetComponentsInChildren<SpriteRenderer>().ToList();
            SetArmActive(true);
        }

        public void SetArmActive(bool active)
        {
            //_armObject.ForEach(gameObject => gameObject.SetActive(active));
        }

        public void Blink(float duration, Ease ease = Ease.Linear)
        {
            foreach (Tween tween in _blinkTweenList)
            {
                tween.Kill();
            }
            _blinkTweenList.Clear();

            SpriteRendererList.ForEach(renderer =>
            {
                renderer.material.SetFloat(_BlinkShanderkHash, 1);
                _blinkTweenList.Add(renderer.material.DOFloat(0, _BlinkShanderkHash, duration).SetEase(ease));
            });
        }

        private void Update()
        {
            if (isUsualFacing && UsualFacingTarget != Vector3.zero)
            {
                LookTarget(UsualFacingTarget);
            }

            if (RotationTrm != null && GameManager.Instance.IsGameOver == false)
            {
                Vector3 center = RotationTrm.position;

                Vector3 mousePos = Hashira.CanvasUI.UIManager.WorldMousePosition;
                mousePos = MainScreenEffect.OriginPositionConvert(mousePos);
                Vector3 targetDir = mousePos - center;

                RotationTrm.right = targetDir;
            }
        }

        public void SetUsualFacingTarget(Vector3 pos)
        {
            UsualFacingTarget = pos;
        }

        public void LookTarget(Vector3 pos)
        {
            if (Mathf.Abs(FacingDirection + Mathf.Sign(Vector3.Dot(pos - _entity.transform.position, _entity.transform.right))) < 1)
                Flip();
        }

        public void LookTarget(float x)
        {
            if (FacingDirection != x)
                Flip();
        }

        public void Flip()
        {
            FacingDirection *= -1;
            VisualTrm.localEulerAngles = new Vector3(0, ((FacingDirection > 0) ^ _onFlip) ? 0 : 180, 0);
            OnFlipEvent?.Invoke(FacingDirection);
        }

        public void Dissolve(float value, float duration = -1, Ease ease = Ease.Linear)
        {
            foreach (Tween tween in _dissolveTweenList)
            {
                tween.Kill();
            }
            _dissolveTweenList.Clear();

            SpriteRendererList.ForEach(renderer =>
            {
                if (duration == -1) renderer.material.SetFloat(_DissolveShader, value);
                else _dissolveTweenList.Add(renderer.material.DOFloat(value, _DissolveShader, duration).SetEase(ease));
            });
        }
    }
}
