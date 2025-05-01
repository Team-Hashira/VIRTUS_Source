using Hashira.Combat;
using Hashira.Core;
using Hashira.Players;
using System;
using UnityEngine;

namespace Hashira.Bosses.Patterns.GiantGolem
{
    public class VoidFloor : MonoBehaviour
    {
        [SerializeField] private GiantGolemPlatformList _giantGolemPlatformList;
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                PlayerMoveToTop();
            }
        }

        private void PlayerMoveToTop()
        {
            Player player = PlayerManager.Instance.Player; 
            var platforms = _giantGolemPlatformList.GetAllPlatforms();
            
            Array.Sort(platforms, (x, y) =>
            {
                float disX = Mathf.Abs(x.transform.position.x-player.transform.position.x);
                float disY = Mathf.Abs(y.transform.position.x-player.transform.position.x);
                
                return disX.CompareTo(disY);
            });
            
            float maxY = float.MinValue;
            GiantGolemPlatform selectedPlatform = null;
            
            for (int i = 0; i < platforms.Length; i++)
            {
                if (platforms[i].transform.position.y > maxY)
                {
                    maxY = platforms[i].transform.position.y;
                    selectedPlatform = platforms[i];
                }
            }

            
            player.transform.position = selectedPlatform.transform.position + new Vector3(0, 2, 0);
            player.EntityHealth.ApplyDamage(AttackInfo.defaultOneDamage, popUpText:false);
        }
    }
}
