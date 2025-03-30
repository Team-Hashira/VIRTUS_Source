using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Hashira.Entities.Components
{
    public enum EEmotion
    {
        Happy,
        Sad, 
        Surprise,
		Question
	}

    public class EntityEmoji : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private SerializedDictionary<EEmotion, Sprite> _emotionDictionary;

        private float _emotionDuration;
        private float _lastEmotionTime;

        public void Initialize(Entity entity)
        {
            _spriteRenderer.sprite = null;
            _emotionDuration = -1;
            _lastEmotionTime = 0;
        }

        private void Update()
        {
            if (_emotionDuration != -1 && _lastEmotionTime + _emotionDuration < Time.time)
                HideEmoji();
        }

        public bool ShowEmoji(EEmotion eEmotion, float duration = -1)
        {
            if (_emotionDictionary.TryGetValue(eEmotion, out Sprite sprite))
            {
                _spriteRenderer.sprite = sprite;
                _emotionDuration = duration;
                _lastEmotionTime = Time.time;
                return true;
            }
            return false;
        }
        public bool HideEmoji()
        {
            if (_spriteRenderer.sprite == null) return false;
            _spriteRenderer.sprite = null;
            _emotionDuration = -1;
            return true;
        }
    }
}
