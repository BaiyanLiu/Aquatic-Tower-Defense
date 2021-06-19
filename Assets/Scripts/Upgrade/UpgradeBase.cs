using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Upgrade
{
    public abstract class UpgradeBase : MonoBehaviour
    {
        private const string HasColor = "#22b14cff";
        private const string NextColor = "#fff200ff";
        private const string FutureColor = "#808080ff";

        public GameObject Prefab;
        public float[] Amount;
        public int[] Cost;

        public abstract string Name { get; }
        protected virtual string AmountName => "Amount";

        private int _level = -1;

        public string FormatDisplayText<T>(string amountName, T[] amount)
        {
            var amountSb = new StringBuilder();
            for (var i = 0; i < amount.Length; i++)
            {
                string color;
                if (i <= _level)
                {
                    color = HasColor;
                } 
                else if (i == _level + 1)
                {
                    color = NextColor;
                }
                else
                {
                    color = FutureColor;
                }

                if (amountSb.Length > 0)
                {
                    amountSb.Append("/");
                }
                amountSb.Append($"<color={color}>{amount[i]}</color>");
            }
            return $"{amountName}: {amountSb}";
        }

        public virtual List<string> GetAmountDisplayText()
        {
            return new List<string> {FormatDisplayText(AmountName, Amount)};
        }
    }
}
