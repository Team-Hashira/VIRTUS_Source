using Hashira.Players;
using UnityEngine;

namespace Hashira.Entities.Components
{
    public class PlayerModule : MonoBehaviour, IEntityComponent
    {
        private Player _player;

        public void Initialize(Entity entity)
        {
            _player = entity as Player;
        }
    }
}
