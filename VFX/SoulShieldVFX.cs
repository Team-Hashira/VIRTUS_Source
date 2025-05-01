using UnityEngine;
using UnityEngine.Serialization;

namespace Hashira.VFX
{
    public class SoulShieldVFX : SimplePoolingObject
    {
        [SerializeField] private Renderer _visualRenderer;
        private readonly int _currentLevelID = Shader.PropertyToID("_CurrentLevel");
        private readonly int _maxLevelID = Shader.PropertyToID("_MaxLevel");
        
        public void SetBrokenLevel(int currentLevel, int maxLevel)
        {
            _visualRenderer.material.SetFloat(_currentLevelID, currentLevel);
            _visualRenderer.material.SetFloat(_maxLevelID, maxLevel);
        }
    }
}
