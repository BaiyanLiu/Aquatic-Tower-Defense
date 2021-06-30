using System;
using UnityEngine;

namespace Assets.Scripts.Persistence
{
    [Serializable]
    public sealed class TowerSnapshot
    {
        public string Name;
        public Vector2 Position;
        public int Level;
        public int Experience;
        public int ExperienceRequired;
        public float DamageDone;
        public int Kills;
        public Attribute<float> SellCost;
    }
}
