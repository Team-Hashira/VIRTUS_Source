using UnityEngine;

namespace Hashira.VFX
{
    public class ParabolaDrawer : MonoBehaviour
    {
        [SerializeField]
        private LineRenderer _lineRenderer;

        public void DrawLine(Vector2 startPosition, Vector2 direction, float power, Vector2 gravity, int resolution)
        {
            _lineRenderer.positionCount = 0;

            float additionalValue = 1 / resolution;

            for (int i = 0; i < resolution; i++)
            {
                float t = additionalValue * i;
                Vector2 pos = MathEx.Parabola(direction, power, gravity, t);
                _lineRenderer.SetPosition(i, pos);
            }
        }

        public void DrawLine(Vector2 startPosition, float angle, float power, Vector2 gravity, int resolution)
        {
            _lineRenderer.positionCount = resolution * 30;

            float additionalValue = 1 / (float)resolution;

            for (int i = 0; i < resolution * 30; i++)
            {
                float t = additionalValue * i;
                Vector2 pos = startPosition + MathEx.Parabola(angle, power, gravity, t);
                _lineRenderer.SetPosition(i, pos);
            }
        }
    }
}
