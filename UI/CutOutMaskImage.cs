using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Hashira.UI
{
    public class CutOutMaskImage : Image
    {
        private static readonly int StencilComp = Shader.PropertyToID("_StencilComp");

        protected override void Start()
        {
            base.Start();
            StartCoroutine(Fix());
        }

        private IEnumerator Fix()
        {
            yield return null;
            maskable = false;
            maskable = true;
        }

        public override Material materialForRendering
        {
            get
            {
                Material mat = new Material(base.materialForRendering);
                mat.SetInt(StencilComp, (int)CompareFunction.NotEqual);
                return mat;
            }
        }
    }
}
