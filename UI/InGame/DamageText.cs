using Crogen.CrogenPooling;
using DG.Tweening;
using TMPro;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Hashira
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private float _upSpeed;
        [SerializeField] private float _gravity;
        [SerializeField] private SimplePoolingObject _simplePoolingObject;

        private TextMeshPro _damageText;
        private float _yVelocity;

        private float _random;

        private void Awake()
        {
            _damageText = transform.GetComponentInChildren<TextMeshPro>();
        }

        public void Init(int damage, float spread = 0.2f)
        {
            Init(damage, Color.white, spread);
        }
        public void Init(int damage, Color color, float spread = 0.2f)
        {
            transform.position += (Vector3)Random.insideUnitCircle * spread;
            transform.localScale = Vector3.one * 0.2f;
            transform.DOKill();
            transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack, 5 * Mathf.Log10(damage + 1));
            _random = Random.Range(0f, 1f) * 100;
            _damageText.text = damage.ToString();
            _damageText.color = color;
            _damageText.alpha = 1.0f;
            _yVelocity = _upSpeed;
        }

        private void Update()
        {
            transform.position += Vector3.up * Time.deltaTime * _yVelocity;
            _yVelocity -= _gravity * Time.deltaTime;
            _damageText.alpha = 1 - _simplePoolingObject.CurLifetime / _simplePoolingObject.duration;
        }
    }
}
