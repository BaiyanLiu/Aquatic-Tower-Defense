using UnityEngine;

namespace Assets.Scripts
{
    public static class Icons
    {
        public static readonly Sprite Damage = GameState.Instance.IconsByName["Icon_Swords"];
        public static readonly Sprite Range = GameState.Instance.IconsByName["Icon_Radar"];
        public static readonly Sprite AttackSpeed = GameState.Instance.IconsByName["Icon_Stopwatch"];
        public static readonly Sprite ProjectileSpeed = GameState.Instance.IconsByName["Icon_Wind"];
        public static readonly Sprite Duration = GameState.Instance.IconsByName["Icon_Hourglass"];
        public static readonly Sprite Frequency = GameState.Instance.IconsByName["Icon_Clock_Rotate_Left"];
    }
}
