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
        
        private Color _originalColor;
        private Material DamageCastSignMaterial
        {
            get
            {
                if (MeshRenderer == null || MeshRenderer.material == null) 
                    MeshRenderer.material = _material;
                
                return MeshRenderer.material.HasInt(_damageCastShapeTypeID) ? MeshRenderer.material : null;
            }
        }

        private void Awake()
        {
            _originalColor = _material.GetColor(_colorID);
        }

        public void SetDamageCastSignValue(float value)
        {
            DamageCastSignMaterial?.DOKill();
            DamageCastSignMaterial.SetFloat(_valueID, value);
        }
    
        public Tween SetDamageCastSignValue(float value, float duration)
        {
            DamageCastSignMaterial?.DOKill();
            return DamageCastSignMaterial.DOFloat(value, _valueID, duration);
        }

        public void SetDamageCastSignColor(Color color)
        {
            DamageCastSignMaterial.SetColor(_colorID, color);
        }
        
        public void SetDamageCastSignOriginColor() => DamageCastSignMaterial.SetColor(_colorID, _originalColor);
        
        [Button("ResetDamageCastVisualSign(Test)")]
        public void ResetDamageCastVisualSign()
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

            MeshRenderer.material = _material;
            MeshFilter.mesh = mesh;
        }
    }
}
