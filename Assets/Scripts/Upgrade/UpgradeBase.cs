using System.Collections.Generic;
using System.Text;
using Assets.Scripts.Tower;
using UnityEngine;

namespace Assets.Scripts.Upgrade
{
    public abstract class UpgradeBase : MonoBehaviour
    {
        public GameObject Prefab;
        public float[] Amount;
        public int[] Cost;

        public abstract string Name { get; }
        protected virtual string AmountName => "Amount";

        protected int Level = -1;

        public string FormatDisplayText<T>(string amountName, T[] amount)
        {
            var amountSb = new StringBuilder();
            for (var i = 0; i < amount.Length; i++)
            {
                if (amountSb.Length > 0)
                {
                    amountSb.Append("/");
                }
                if (i == Level)
                {
                    amountSb.Append("<b>");
                }
                amountSb.Append(amount[i]);
                if (i == Level)
                {
                    amountSb.Append("</b>");
                }
            }
            return $"{amountName}: {amountSb}";
        }

        public abstract void Apply(TowerBase tower);

        public virtual List<string> GetAmountDisplayText()
        {
            return new List<string> {FormatDisplayText(AmountName, Amount)};
        }
    }
}
