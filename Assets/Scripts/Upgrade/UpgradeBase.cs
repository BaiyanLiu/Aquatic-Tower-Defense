using System;
using System.Collections.Generic;
using System.Globalization;
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
        public TowerBase Tower { protected get; set; }

        private bool HasNextLevel => Cost.Length > Level + 1;
        public int? NextCost => HasNextLevel ? (int?) Cost[Level + 1] : null;
        public bool CanLevelUp => HasNextLevel && _gameState.Gold >= Cost[Level + 1];

        private GameState _gameState;
        protected int Level = -1;

        [UsedImplicitly]
        private void Start()
        {
            _gameState = GameState.GetGameState(gameObject);
        }

        public void LevelUp()
        {
            if (!CanLevelUp)
            {
                return;
            }
            var cost = Cost[Level + 1];
            _gameState.UpdateGold(-cost);
            Tower.SellCost.Base += cost / 2f;
            Level++;
            Tower.UpdateStats();
        }

        public void Apply()
        {
            if (Level == -1)
            {
                return;
            }
            OnApply();
        }

        public abstract void OnApply();

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
                amountSb.Append($"<color={color}>{(isCost ? DefaultFormatAmountText(amount[i]) : FormatAmountText(amount[i]))}</color>");
            }
            return $"{amountName}: {amountSb}";
        }

        protected virtual string FormatAmountText<T>(T amount)
        {
            return DefaultFormatAmountText(amount);
        }

        private static string DefaultFormatAmountText<T>(T amount)
        {
            return Convert.ToString(amount, CultureInfo.InvariantCulture);
        }

        public virtual List<string> GetAmountDisplayText()
        {
            return new List<string> {FormatDisplayText(AmountName, Amount, false)};
        }
    }
}
