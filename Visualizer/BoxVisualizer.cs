using Crogen.CrogenPooling;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Visualizers
{
    public class BoxVisualizer : MonoBehaviour, IPoolingObject
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        [SerializeField]
        private Transform _maskTrm;

        public string OriginPoolType { get; set; }
        GameObject IPoolingObject.gameObject { get; set; }

        private Vector2 _startPos;
        private Vector2 _endPos;

        private List<IPoolingObject> _directionMarkList;
        private Vector2 _offset;
        private Color _defaultLineColor;

        private Coroutine _lifeTimeCoroutine;
        private Coroutine _foldCoroutine;

        private void Awake()
        {
            _defaultLineColor = _spriteRenderer.color;
            _directionMarkList = new List<IPoolingObject>();
        }

        public void Visualize(Vector2 startPos, Vector2 endPos, float width, float lifeTime, float duration = 0, Func<float, float> EaseFunction = null, Action OnComplete = null)
        {
            //일단 보이게 해주고
            _spriteRenderer.enabled = true;
            _startPos = startPos;
            _endPos = endPos;

            //회전값과 거리를 구할 방향 구해주고
            Vector2 direction = _endPos - _startPos;
            float distance = direction.magnitude;

            //피봇과 사이즈 기타 등등 맞춰주고.
            _offset = new Vector2(_startPos.x + direction.x * 0.5f, _startPos.y + direction.y * 0.5f);

            //DirectionMark 생성할 숫자 구하고
            int directionMarkCount = Mathf.CeilToInt(distance / width);

            //이제 정규화된 방향 벡터가 필요하니까 정규화
            direction.Normalize();
            //오프셋 맞춰주고
            _offset += -direction * width * 0.5f;

            //방향 회전값 구해주고 회전해주고
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            //그에 맞게 DirectionMark 생성
            CreateDirectionMark(_startPos, _endPos, distance, angle, directionMarkCount, width);


            //애니메이션 적용
            if (duration > 0)
            {
                _spriteRenderer.size = new Vector2(0, width);
                _maskTrm.localScale = new Vector2(0, width);
                StartCoroutine(VisualizeCoroutine(_startPos, _endPos, duration, EaseFunction, OnComplete));
            }
            else
            {
                transform.position = _offset;
                _spriteRenderer.size = new Vector2(distance, width);
                _maskTrm.localScale = new Vector2(distance, width);
                OnComplete?.Invoke();
            }

            _lifeTimeCoroutine = StartCoroutine(LifeTimeCoroutine(lifeTime));

            _foldCoroutine = null;
        }

        private IEnumerator VisualizeCoroutine(Vector2 startPos, Vector2 endPos, float duration, Func<float, float> EaseFunction, Action OnComplete)
        {
            Vector2 direction = endPos - startPos;
            float distance = direction.magnitude;

            float percent = 0;
            float multiplier = 1f / duration;

            Vector2 offset = direction.normalized * _spriteRenderer.size.y * -0.5f;

            endPos += offset;
            startPos += offset;

            do
            {
                float easedValue = EaseFunction == null ? percent : EaseFunction(percent);
                _spriteRenderer.size = new Vector2(Mathf.Lerp(0, distance, easedValue), _spriteRenderer.size.y);
                _spriteRenderer.color = Color.Lerp(Color.white, _defaultLineColor, Mathf.Pow(easedValue, 2));
                _maskTrm.localScale = new Vector2(Mathf.Lerp(0, distance, easedValue), _spriteRenderer.size.y);
                transform.position = Vector2.Lerp(startPos, endPos, easedValue * 0.5f);
                percent += Time.deltaTime * multiplier;
                yield return null;
            }
            while (percent < 1f);
            _spriteRenderer.size = new Vector2(distance, _spriteRenderer.size.y);
            _spriteRenderer.color = _defaultLineColor;
            _maskTrm.localScale = new Vector2(distance, _spriteRenderer.size.y);
            transform.position = Vector2.Lerp(startPos, endPos, 0.5f);
            OnComplete?.Invoke();
        }

        public void Fold(float duration)
        {
            if (_lifeTimeCoroutine != null)
                StopCoroutine(_lifeTimeCoroutine);
            if (_foldCoroutine != null)
                return;
            StartCoroutine(FoldCoroutine(transform.position, _endPos, duration));
        }

        private IEnumerator FoldCoroutine(Vector2 startPos, Vector2 endPos, float duration, Func<float, float> EaseFunction = null)
        {
            Vector2 direction = endPos - startPos;
            float distance = direction.magnitude;

            float percent = 0.5f;
            float multiplier = 1f / duration;

            Vector2 offset = direction.normalized * _spriteRenderer.size.y * -0.5f;

            endPos += offset;
            startPos += offset;
            do
            {
                float easedValue = EaseFunction == null ? percent : EaseFunction(percent);
                _spriteRenderer.size = new Vector2(Mathf.Lerp(distance, 0, easedValue), _spriteRenderer.size.y);
                //_spriteRenderer.color = Color.Lerp(Color.white, _defaultLineColor, Mathf.Pow(easedValue, 2));
                _maskTrm.localScale = new Vector2(Mathf.Lerp(distance, 0, easedValue), _spriteRenderer.size.y);
                transform.position = Vector2.Lerp(startPos, endPos, easedValue);
                percent += Time.deltaTime * multiplier;
                yield return null;
            }
            while (percent < 1f);
            _spriteRenderer.size = new Vector2(0, _spriteRenderer.size.y);
            //_spriteRenderer.color = _defaultLineColor;
            _maskTrm.localScale = new Vector2(0, _spriteRenderer.size.y);
            transform.position = endPos;
            this.Push();
        }

        private void CreateDirectionMark(Vector2 startPos, Vector2 endPos, float distance, float angle, int count, float space)
        {
            count++;
            float additionalValue = 1f / count;
            float t = 0;
            for (int i = 0; i < count; i++)
            {
                SimplePoolingObject directionMark = PopCore.Pop(WorldUIPoolType.DirectionMark, Vector2.Lerp(startPos, endPos, t), Quaternion.Euler(0, 0, angle)) as SimplePoolingObject;
                directionMark.transform.localScale = Vector2.one * space;
                _directionMarkList.Add(directionMark);
                t += additionalValue;
            }
        }

        /// <summary>
        /// 빤짝 거립니다.
        /// </summary>
        /// <param name="toblinkDuration">blinkColor까지 도달하는 시간</param>
        /// <param name="toDefaultDuration">blinkColor에서 defautlColor로 돌아오는 시간</param>
        /// <param name="blinkColor">적지 않을경우 기본은 흰색</param>
        public void Blink(float toblinkDuration, float toDefaultDuration, Color blinkColor = default, Action OnComplete = null)
        {
            if (blinkColor == default)
                blinkColor = Color.white;

            _spriteRenderer.DOColor(blinkColor, toblinkDuration)
                .OnComplete(() => _spriteRenderer.DOColor(_defaultLineColor, toDefaultDuration).OnComplete(() => OnComplete?.Invoke()));
        }

        private IEnumerator LifeTimeCoroutine(float lifeTime)
        {
            if (lifeTime < 0)
                yield break;
            yield return new WaitForSeconds(lifeTime);
            _foldCoroutine = StartCoroutine(FoldCoroutine(transform.position, _endPos, 0.3f));
        }

        public void OnPop()
        {
            _spriteRenderer.enabled = false;
        }

        public void OnPush()
        {
            _directionMarkList.ForEach(mark => mark.Push());
            _directionMarkList.Clear();
        }
    }
}
