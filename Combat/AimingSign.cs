
using UnityEngine;

namespace Hashira.Combat
{
    public class AimingSign : MonoBehaviour
    {
        [SerializeField] private InputReaderSO _inputReader;

        private void LateUpdate()
        {
            var pos = Camera.main.ScreenToWorldPoint(_inputReader.MousePosition);
            transform.position = new Vector3(pos.x, pos.y, 0);
        }
    }
}
