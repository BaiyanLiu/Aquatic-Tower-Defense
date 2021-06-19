using System.Collections.Generic;
using System.Text;
using Assets.Scripts.Tower;
using UnityEngine;

namespace Assets.Scripts.Upgrade
{
    public abstract class UpgradeBase : MonoBehaviour
    {
        public float[] Amount;
        public int[] Cost;

        public int Level { get; private set; } = -1;
        public abstract string Name { get; }
        public virtual string AmountName => "Amount";

        public static string FormatDisplayText<T>(string name, T[] amount, int level)
        {
            var amountSb = new StringBuilder();
            for (var i = 0; i < amount.Length; i++)
            {
                if (amountSb.Length > 0)
                {
                    amountSb.Append("/");
                }
                if (i == level)
                {
                    amountSb.Append("<b>");
                }
                amountSb.Append(amount[i]);
                if (i == level)
                {
                    amountSb.Append("</b>");
                }
            }
            return $"{name}: {amountSb}";
        }

        public int LevelUp()
        {
            return Cost[++Level];
        }

        public abstract void Apply(TowerBase tower);

        public virtual List<string> GetAmountDisplayText(bool includeGain)
        {
            return new List<string> {FormatDisplayText(AmountName, Amount, Level)};
        }
    }
}
