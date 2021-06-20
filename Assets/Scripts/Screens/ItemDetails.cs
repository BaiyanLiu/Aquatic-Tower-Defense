using Assets.Scripts.Effect;
using Assets.Scripts.Item;
using UnityEngine;

namespace Assets.Scripts.Screens
{
    public sealed class ItemDetails : DetailsScreen<ItemBase>
    {
        protected override EffectBase[] TargetEffects => Base.Effects;

        private SpriteRenderer _spriteRenderer;

        protected override float OnUpdate(float height)
        {
            NameText.text = Base.Name;
            return height;
        }

        protected override void OnDeselected()
        {
            _spriteRenderer.color = Color.white;
        }

        protected override void OnSelected()
        {
            _spriteRenderer = Target.GetComponent<SpriteRenderer>();
            _spriteRenderer.color = GameState.ValidColor;
        }
    }
}
