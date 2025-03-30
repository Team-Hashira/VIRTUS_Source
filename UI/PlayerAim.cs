using UnityEngine;
using UnityEngine.UI;

namespace Hashira
{
    public class PlayerAim : MonoBehaviour
    {
        [SerializeField] private Image _circleAim;
        [SerializeField] private RectTransform _crossAim;
        [SerializeField] private InputReaderSO _inputReader;
        private Image[] _crossAims = new Image[4];
        private Vector2[] _crossAimStartPositions = new Vector2[4];
        private float _startCircleSize;

        private RectTransform rectTransform;
        //private EntityWeaponHolder _playerGunWeapon; //기본 총기 만들때 다시 구현 (총기가 애임을 봐야함) 재장전 공격 가능 등등도 구현해야해


        [Header("Color setting")]
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _delayColor;
        [SerializeField] private Color _inactiveColor;

        private void Awake()
        {
            rectTransform = transform as RectTransform;
            _startCircleSize = _circleAim.rectTransform.sizeDelta.x;

            for (int i = 0; i < 4; i++)
            {
                _crossAims[i] = _crossAim.GetChild(i).GetComponent<Image>();
                _crossAimStartPositions[i] = _crossAims[i].rectTransform.anchoredPosition;
            }
        }

        private void Update()
        {
            if (_inputReader.MousePosition != Vector2.zero)
            {
                rectTransform.anchoredPosition = _inputReader.MousePosition;
            }

            //SetSize(_playerGunWeapon.Recoil * 0.2f + 1);
            //if (_playerGunWeapon.CurrentItem is GunWeapon gun && gun != null)
            //    SetColor(gun.BulletAmount > 0 && !_playerGunWeapon.IsReloading && _playerGunWeapon.gameObject.activeSelf && _playerGunWeapon.IsStuck == false ?
            //        (gun.IsCanFire ? _defaultColor : _delayColor) : _inactiveColor);
            //else
            //    SetColor(Color.white);
        }

        public void SetSize(float size)
        {
            _circleAim.rectTransform.sizeDelta = Vector2.one * (size * _startCircleSize);
            for (int i = 0; i < 4; i++)
            {
                _crossAims[i].rectTransform.anchoredPosition = _crossAimStartPositions[i] * size;
            }
        }

        public void SetColor(Color color)
        {
            _circleAim.color = color;
            for (int i = 0; i < 4; i++)
            {
                _crossAims[i].color = color;
            }
        }
    }
}
