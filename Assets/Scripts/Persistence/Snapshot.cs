using System;

namespace Assets.Scripts.Persistence
{
    [Serializable]
    public sealed class Snapshot
    {
        public int Gold;
        public int Lives;
        public int Wave;
        public TowerSnapshot[] Towers;
        public ItemSnapshot[] Items;
    }
}
