using Crogen.CrogenPooling;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Hashira.VFX
{
    public class MagicAreaVFX : MonoBehaviour, IPoolingObject
    {
        [SerializeField] private ParticleSystem _particleSystem;
        
        public string OriginPoolType { get; set; }
        GameObject IPoolingObject.gameObject { get; set; }
        
        public void OnPop()
        {
            _particleSystem.Simulate(0);
            _particleSystem.Play(true);    
        }

        public void DelayPush()
        {
            _particleSystem.Stop(true);
            StartCoroutine(CoroutineDelayPush());
        }

        private IEnumerator CoroutineDelayPush()
        {
            yield return new WaitForSeconds(1);
            this.Push();
        }
        
        public void OnPush()
        {
        }
    }
}
