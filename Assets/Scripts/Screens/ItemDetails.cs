using Assets.Scripts.Effect;
using Assets.Scripts.Item;

namespace Assets.Scripts.Screens
{
    public class ItemDetails : DetailsScreen<ItemBase>
    {
        protected override EffectBase[] TargetEffects => Base.Effects;

        protected override float OnUpdate(float height)
        {
            NameText.text = Base.Name;
            return height;
        }
    }
}
