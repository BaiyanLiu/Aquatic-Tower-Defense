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

        public GameObject Icon;
        public float[] Amount;
        public int[] Cost;

        public abstract string Name { get; }
        protected virtual string AmountName => "Amount";
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
            GameState.Instance.UpdateSnapshot();
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

        public string FormatDisplayText<T>(string amountName, T[] amount, bool isCost)
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
                amountSb.Append($"<color={color}>{amount[i]}{(isCost ? "" : AmountUnit)}</color>");
            }
            return $"{amountName}: {amountSb}";
        }

        public virtual List<string> GetAmountDisplayText()
        {
            return new List<string> {FormatDisplayText(AmountName, Amount, false)};
        }
    }
}
