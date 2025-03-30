using DG.Tweening;
using Hashira.Core;
using Hashira.Pathfind;
using UnityEngine;
using Hashira.StageSystem;

namespace Hashira.MainScreen
{
    public class MainScreenEffect : MonoBehaviour
    {
        private static Material _mainScreenMat;
        private static Transform _levelTransform;
        private static Transform _transform;
        private static Stage _currentStage;
        private static MainScreenEffect _mainScreenEffect;

        public static Transform GetLevelTransform() => _levelTransform;
        public static Transform GetTransform() => _transform;

        private static Vector3 _originScale;
        private static Vector2 _originPos;
        private static Vector2 _scaleMulipler = Vector2.one;
        private static Vector2 _movableAreaMin => Camera.main.ViewportToWorldPoint(Vector2.zero) + (Vector3)(_currentStage.Scale * _scaleMulipler) - (Vector3)_currentStage.Center;
        private static Vector2 _movabletAreaMax => Camera.main.ViewportToWorldPoint(Vector2.one) - (Vector3)(_currentStage.Scale * _scaleMulipler) - (Vector3)_currentStage.Center;
        public static Vector2 worldScreenPositionMin => _currentStage.Center - _currentStage.Scale;
        public static Vector2 worldScreenPositionMax => _currentStage.Center + _currentStage.Scale;

        private static readonly int _playerPos = Shader.PropertyToID("_PlayerPos");

        // WallShockWave
        private static readonly int _wallShockWave_ValueID = Shader.PropertyToID("_WallShockWave_Value");
        private static readonly int _wallShockWave_StrengthID = Shader.PropertyToID("_WallShockWave_Strength");
        private static readonly int _wallShockWave_MaxID = Shader.PropertyToID("_WallShockWave_Max");
        private static readonly int _wallShockWave_MinID = Shader.PropertyToID("_WallShockWave_Min");

        // Glitch 
        private static readonly int _glitch_ValueID = Shader.PropertyToID("_Glitch_Value");

        // Alpha
        private static readonly int _alphaID = Shader.PropertyToID("_Alpha");

        // Tweens
        private static Tween _moveTween;
        private static Tween _rotateTween;
        private static Tween _reverseXTween;
        private static Tween _reverseYTween;
        private static Tween _scalingTween;
        private static Tween _glitchTween;
        private static Tween _fadeTween;

        private void Awake()
        {
            _levelTransform = FindFirstObjectByType<StageSystem.Stage>().transform;
            _transform = this.transform;
            _mainScreenEffect = this;
            _mainScreenMat = GetComponent<MeshRenderer>().material;
        }

        private void Start()
        {
            _currentStage = StageGenerator.Instance.GetCurrentStage();
            _originScale = _transform.localScale;

            _mainScreenMat.SetFloat(_glitch_ValueID, 1f);
            _mainScreenMat.SetFloat(_alphaID, 0f);

            OnAlpha(1, 0.75f, Ease.InSine);
            OnGlitch(0f, 0.8f, Ease.InCubic);
            OnScaling(1, 0.45f, Ease.Unset);
        }

        private void Update()
        {
            var playerPos = PlayerManager.Instance.Player.transform.position;
            SetPlayerPos(playerPos);

#if UNITY_EDITOR
            Debug();
#endif
        }


        public static Vector3 OriginPositionConvert(Vector3 position)
        {
            Vector3 offsetPos = position - _transform.position;
            Vector3 rotatedPos = Quaternion.Inverse(_transform.rotation) * offsetPos;
            Vector3 scaledPosx= new Vector3(
                rotatedPos.x / (_transform.localScale.x / _originScale.x),
                rotatedPos.y / (_transform.localScale.y / _originScale.y));

            return scaledPosx;
        }

        private void Debug()
        {
            if (Input.GetKey(KeyCode.Alpha0))
            {
                OnRotate(0);
            }
            if (Input.GetKey(KeyCode.Alpha1))
            {
                OnRotate(35);
            }
            if (Input.GetKey(KeyCode.Alpha2))
            {
                OnRotate(180);
            }

            if (Input.GetKey(KeyCode.Alpha3))
            {
                OnScaling();
            }
            if (Input.GetKey(KeyCode.Alpha4))
            {
                OnScaling(0.5f);
            }
            if (Input.GetKey(KeyCode.Alpha5))
            {
                OnScaling(0.25f);
            }


            if (Input.GetKeyDown(KeyCode.UpArrow))
                OnLocalMoveScreenSide(DirectionType.Up);

            if (Input.GetKeyDown(KeyCode.DownArrow))
                OnLocalMoveScreenSide(DirectionType.Down);

            if (Input.GetKeyDown(KeyCode.RightArrow))
                OnLocalMoveScreenSide(DirectionType.Right);

            if (Input.GetKeyDown(KeyCode.LeftArrow))
                OnLocalMoveScreenSide(DirectionType.Left);

            if (Input.GetKeyDown(KeyCode.Backspace))
                OnLocalMoveScreenSide(DirectionType.Zero);

            if (Input.GetKeyDown(KeyCode.Equals))
                OnReverseX();

            if (Input.GetKeyDown(KeyCode.Minus))
                OnReverseY();
        }

        private void OnDestroy()
        {
            ResetTrm();
        }

        private void ResetTrm()
        {
            _transform.position = Vector3.zero;
            _transform.rotation = Quaternion.identity;
            _transform.localScale = _originScale;
        }

        private void SetPlayerPos(Vector2 pos) => _mainScreenMat.SetVector(_playerPos, pos);

        //public void OnShockWaveEffect(Vector2 pos)
        //{
        //    SetPlayerPos(pos);
        //    _mainScreenMat.DOKill();
        //    _mainScreenMat.SetFloat(_simpleShckWave_ValueID, 0);
        //    _mainScreenMat.DOFloat(4, _simpleShckWave_ValueID, 1f);
        //}

        public static void OnWallShockWaveEffect(Vector2 pos)
        {
            Vector2 abs = new Vector2(Mathf.Abs(pos.x), Mathf.Abs(pos.y));
            Vector2 normal = new Vector2(Mathf.Sign(pos.x), Mathf.Sign(pos.y)) * 0.5f + Vector2.one * 0.5f;

            Vector2 max = new Vector2(abs.x > abs.y ? normal.x : 0, abs.x < abs.y ? normal.y : 0);
            Vector2 min = new Vector2(
                abs.x > abs.y ? normal.x > 0.5f ? normal.x - 0.1f : normal.x + 0.1f : 0,
                abs.x < abs.y ? normal.y > 0.5f ? normal.y - 0.1f : normal.y + 0.1f : 0);

            Sequence seq = DOTween.Sequence();
            seq.AppendCallback(() =>
            {
                _mainScreenMat.SetVector(_wallShockWave_MaxID, max);
                _mainScreenMat.SetVector(_wallShockWave_MinID, min);

                _mainScreenMat.SetFloat(_wallShockWave_StrengthID, 0.5f);
                _mainScreenMat.SetFloat(_wallShockWave_ValueID, 0);

            });
            seq.Append(_mainScreenMat.DOFloat(1, _wallShockWave_ValueID, 1.75f));
            seq.Append(_mainScreenMat.DOFloat(0, _wallShockWave_StrengthID, 0.1f));
        }

        public static void OnRotate(float value)
        {
            OnRotate(value, 0.25f);
        }
        public static Tween OnRotate(float value, float duration = 0.25f, RotateMode rotateMode = RotateMode.Fast, Ease ease = Ease.OutBounce)
        {
            _rotateTween?.Kill();
            PlayerManager.Instance.Player.Rotate(-value, duration, rotateMode, ease);
            return _rotateTween = _transform.DORotate(new Vector3(0, 0, value), duration, rotateMode).SetEase(ease);
        }

        public static void OnLocalMoveScreenSide(int direction)
        {
            OnLocalMoveScreenSide((DirectionType)direction);
        }
        /// <summary>
        /// Only Use Up, Right, Down, Left, Zero
        /// </summary>
        /// <param name="directionType"></param>
        public static Tween OnLocalMoveScreenSide(DirectionType directionType, float duration = 0.25f, Ease ease = Ease.OutBounce)
        {
            return OnLocalMoveScreenSide(Direction2D.GetDirection(directionType), duration, ease);
        }
        public static Tween OnLocalMoveScreenSide(Vector2 direction, float duration = 0.25f, Ease ease = Ease.OutBounce)
        {
            Vector2 finalViewport = Vector2.one * 0.5f;

            if (Mathf.Approximately(direction.sqrMagnitude, 0f) == false)
            {
                direction = direction.normalized * float.MaxValue;
                Vector2 curViewportPos = Camera.main.WorldToViewportPoint(_transform.position);
                finalViewport = new Vector2(
                    Mathf.Clamp01(curViewportPos.x + direction.x),
                    Mathf.Clamp01(curViewportPos.y + direction.y));
            }

            return OnMoveScreenSide(finalViewport, duration, ease);
        }
        public static Tween OnMoveScreenSide(Vector2 viewPort, float duration = 0.25f, Ease ease = Ease.OutBounce)
        {
            Vector2 worldPos = Camera.main.ViewportToWorldPoint(viewPort);

            worldPos = new Vector2(
                Mathf.Clamp(worldPos.x, _movableAreaMin.x, _movabletAreaMax.x),
                Mathf.Clamp(worldPos.y, _movableAreaMin.y, _movabletAreaMax.y));

            return OnMove(worldPos, duration, ease);
        }
        public static Tween OnMove(Vector2 pos, float duration = 0.25f, Ease ease = Ease.OutBounce)
        {
            _moveTween?.Kill();

            _moveTween = _transform.DOMove(pos, duration).SetEase(ease)
                .OnComplete(()=>_originPos = _transform.position);

            return _moveTween;
        }

        public static void OnScaling(float scale)
        {
            OnScaling(scale, 0.25f);
        }
        public static Tween OnScaling(float scale = 1, float duration = 0.25f, Ease ease = Ease.OutBounce)
        {
            _scalingTween?.Kill();
            _scaleMulipler = Vector2.one * scale;
            return _scalingTween = _transform.DOScale(_originScale * _scaleMulipler, duration).SetEase(ease);
        }

        public static void OnReverseX()
        {
            _reverseXTween?.Kill();
            _reverseXTween = _transform.DOScaleX(_transform.localScale.x * -1, 0.25f);
        }
        public static void OnReverseY()
        {
            _reverseYTween?.Kill();
            _reverseYTween = _transform.DOScaleY(_transform.localScale.y * -1, 0.25f);
        }

        public static void OnGlitch(float value)
        {
            OnGlitch(value, 0.25f);
        }
        public static Tween OnGlitch(float value, float duration = 0.25f, Ease ease = Ease.Unset)
        {
            _glitchTween?.Kill();
            return _glitchTween = _mainScreenMat.DOFloat(value, _glitch_ValueID, duration).SetEase(ease);
        }

        public static void OnAlpha(float value)
        {
            OnAlpha(value, 0.25f);
        }
        public static void OnAlpha(float value, float duration=0.25f, Ease ease = Ease.Unset)
        {
            _fadeTween?.Kill();
            _fadeTween = _mainScreenMat.DOFloat(value, _alphaID, duration).SetEase(ease);
        }
    }
}
