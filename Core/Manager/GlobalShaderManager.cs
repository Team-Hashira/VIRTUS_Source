using UnityEngine;

namespace Hashira
{
    public class GlobalShaderManager : MonoSingleton<GlobalShaderManager>
    {
        private readonly int _shaderUnscaledtime = Shader.PropertyToID("_ShaderUnscaledtime");

        void Start()
        {
        
        }

        void Update()
        {
            SetShaderUnscaledtime();
        }

        private void SetShaderUnscaledtime() => Shader.SetGlobalFloat(_shaderUnscaledtime, Time.unscaledTime);
    }
}
