using UnityEngine;

namespace Assets.Scripts.Effect.Area
{
    public sealed class AreaSlowEffect : EnemyAreaEffect
    {
        public override string Name => "Intimidation";
        public override Color StatusColor => Colors.Instance.Blue;
    }
}
