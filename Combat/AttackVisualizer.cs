using Crogen.AttributeExtension;
using DG.Tweening;
using System;
using UnityEngine;

namespace Hashira.Combat
{
    public class AttackVisualizer : MonoBehaviour
    {
        [field:SerializeField] public DamageCaster2D DamageCaster { get; private set; }
        [SerializeField] private Material _material;
        private MeshRenderer _meshRenderer;
        private MeshFilter _meshFilter;
        private MeshFilter MeshFilter
        {
            get
            {
                _meshFilter ??= GetComponent<MeshFilter>();
                if (_meshFilter == null) _meshFilter = gameObject.AddComponent<MeshFilter>();
                return _meshFilter;
            }
        }
        private MeshRenderer MeshRenderer
        {
            get
            {
                _meshRenderer ??= GetComponent<MeshRenderer>();
                if (_meshRenderer == null) _meshRenderer = gameObject.AddComponent<MeshRenderer>();
                return _meshRenderer;
            }
        }
        
        private readonly int _damageCastShapeTypeID = Shader.PropertyToID("_DAMAGECASTSHAPETYPE");
        private readonly int _valueID = Shader.PropertyToID("_Value");
        private readonly int _colorID = Shader.PropertyToID("_Color");
        private readonly int _alphaID = Shader.PropertyToID("_Alpha");
        
        private Material DamageCastSignMaterial
        {
            get
            {
                if (MeshRenderer == null || MeshRenderer.material == null)
                {
                    MeshRenderer.material = Instantiate(_material);
                    _originalColor = _material.GetColor(_colorID);
                }
                
                return MeshRenderer.material.HasInt(_damageCastShapeTypeID) ? MeshRenderer.material : null;
            }
        }

        private bool _isBlink = false;
        private float _blinkTime = 0;
        private float _blinkDelay = 0.1f;
        private float _duration;
        private float _time = 0f;
        private readonly float _blinkMaxDelay = 0.075f;
        private Color _originalColor;
        
        private Sequence _blinkSequence;

        private void OnDestroy()
        {
            DamageCastSignMaterial?.DOKill();
            _blinkSequence?.Kill();
        }

        public void SetDamageCastValue(float value)
        {
            DamageCastSignMaterial?.DOKill();
            DamageCastSignMaterial?.SetFloat(_valueID, value);
        }
        public Tween SetDamageCastValue(float value, float duration)
        {
            DamageCastSignMaterial?.DOKill();
            return DamageCastSignMaterial?.DOFloat(value, _valueID, duration);
        }

        public void SetAlpha(float alpha)
        {
            DamageCastSignMaterial?.SetFloat(_alphaID, alpha);    
        }
        public float GetAlpha()
        {
            return DamageCastSignMaterial.GetFloat(_alphaID);
        }
        public Tween Blink(float duration = 1)
        {
            _time = 0;
            _blinkDelay = _blinkMaxDelay;
            _duration = duration;
            _blinkTime = Time.time;
            _blinkSequence = DOTween.Sequence();
            _blinkSequence.AppendCallback(()=>_isBlink = true);
            _blinkSequence.AppendInterval(duration);
            _blinkSequence.AppendCallback(()=>
            {
                _isBlink = false;
                SetAlpha(1);
                DamageCastSignMaterial.SetColor(_colorID, Color.white * 4);
            });
            _blinkSequence.AppendInterval(Time.deltaTime);
            _blinkSequence.AppendCallback(SetOriginColor);
            return _blinkSequence;
        }

        float EaseInCubic(float x){
            return x * x * x;
        }
        
        private void Update()
        {
            // Blink
            if (_isBlink)
            {
                _time += Time.deltaTime;
                _blinkDelay = Mathf.Lerp(_blinkMaxDelay, 0, EaseInCubic(_time/_duration));
                SetAlpha(Mathf.Cos(_time * Mathf.PI * (1/_blinkMaxDelay)) * 0.5f + 0.5f);
            }
        }

        public void SetOriginColor()
        {
            DamageCastSignMaterial?.SetColor(_colorID, _originalColor);
        }
        
        public void SetDamageCastSignColor(Color color)
        {
            DamageCastSignMaterial.SetColor(_colorID, color);
        }
        
        [Button("InitDamageCastVisualSign(Test)")]
        public void InitDamageCastVisualSign()
        {
            Vector2 center = Vector2.zero;
            Vector2 size = Vector2.zero;

            if (DamageCaster is BoxDamageCaster2D boxDamageCaster)
            {
                center = boxDamageCaster.center;
                size = boxDamageCaster.size;
            }
            else if (DamageCaster is CircleDamageCaster2D circleDamageCaster)
            {
                center = circleDamageCaster.center;
                size = Vector2.one * circleDamageCaster.radius;
            }
            CreateMesh(center, size);
        }

        private void OnDisable()
        {
            _blinkSequence?.Kill();
        }

        private void CreateMesh(Vector2 center, Vector2 size)
        {
            Mesh mesh = new Mesh();
            mesh.Clear();
            Vector3[] vertices = new Vector3[4]
            {
                new Vector3(center.x - size.x / 2, center.y - size.y / 2, 0),
                new Vector3(center.x + size.x / 2, center.y - size.y / 2, 0),
                new Vector3(center.x - size.x / 2, center.y + size.y / 2, 0),
                new Vector3(center.x + size.x / 2, center.y + size.y / 2, 0)
            };
            int[] triangles = new int[]
            {
                0, 2, 1, // 첫 번째 삼각형
                2, 3, 1  // 두 번째 삼각형
            };
            Vector2[] uvs = new Vector2[4]
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(1, 1)
            };
            // 노멀 벡터 설정 (기본적으로 위쪽을 향하도록)
            Vector3[] normals = new Vector3[]
            {
                Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward
            };
        
            mesh.SetVertices(vertices);
            mesh.triangles = triangles;
            mesh.SetUVs(0, uvs);
            mesh.SetNormals(normals);

            MeshRenderer.material = Instantiate(_material);
            _originalColor = _material.GetColor(_colorID);
            MeshFilter.mesh = mesh;
        }
    }
}
