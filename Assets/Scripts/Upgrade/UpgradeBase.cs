using System.Collections.Generic;
using System.Text;
using Assets.Scripts.Tower;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Upgrade
{
    public abstract class UpgradeBase : MonoBehaviour
    {
        private const string HasColor = "#22b14cff";
        private const string NextColor = "#fff200ff";
        private const string FutureColor = "#808080ff";

        public string Name;
        public GameObject Icon;
        public float[] Amount;
        public int[] Cost;

        protected virtual string AmountUnit => "";
        public virtual Color NameColor => Color.white;
        public TowerBase Tower { protected get; set; }
        public int Level { get; set; } = -1;
        public bool IsLoading { protected get; set; }

        private bool HasNextLevel => Cost.Length > Level + 1;
        public int? NextCost => HasNextLevel ? (int?) Cost[Level + 1] : null;
        public bool CanLevelUp => HasNextLevel && GameState.Instance.Gold >= Cost[Level + 1];

        [UsedImplicitly]
        private void Start()
        {
            OnStart();
            if (Level >= 0 && IsLoading)
            {
                OnLevelUp();
                IsLoading = false;
            }
        }

        protected virtual void OnStart() {}

        public void LevelUp()
        {
            if (!CanLevelUp)
            {
                return;
            }
            var cost = Cost[Level + 1];
            GameState.Instance.UpdateGold(-cost);
            Tower.SellCost.Base += cost / 2f;
            Level++;
            OnLevelUp();
            Tower.UpdateStats();
        }

        protected virtual void OnLevelUp() {}

        public void Apply()
        {
            if (Level == -1)
            {
                return;
            }
            OnApply();
        }

        public virtual void OnApply() {}

        public string FormatDisplayText<T>(T[] amount, bool includeUnit = true)
        {
            var amountSb = new StringBuilder();
            for (var i = 0; i < amount.Length; i++)
            {
                string color;
                if (i <= Level)
                {
                    color = HasColor;
                } 
                else if (i == Level + 1)
                {
                    color = NextColor;
                }
                else
                {
                    color = FutureColor;
                }

                if (amountSb.Length > 0)
                {
                    amountSb.Append(" / ");
                }
                var unit = includeUnit ? AmountUnit : "";
                amountSb.Append($"<color={color}>{amount[i]}{unit}</color>");
            }
            return amountSb.ToString();
        }

        public virtual List<string> GetAmountDisplayText()
        {
            return new List<string> {FormatDisplayText(Amount)};
        }

        public abstract List<Sprite> GetAmountIcon();
    }
}
