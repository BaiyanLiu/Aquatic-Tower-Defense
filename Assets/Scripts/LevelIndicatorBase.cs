using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class LevelIndicatorBase : MonoBehaviour
    {
        protected abstract int Level { get; }
        protected abstract int MaxLevel { get; }

        private SpriteRenderer _spriteRenderer;
        private Color _delta;

        [UsedImplicitly]
        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _delta = (Colors.Instance.Yellow - Color.white) / (MaxLevel - 1);
            UpdateColor();
            OnStart();
        }

        protected virtual void OnStart() {}
        
        public void UpdateColor()
        {
            _spriteRenderer.color = Color.white + _delta * Level;
        }
    }
}
