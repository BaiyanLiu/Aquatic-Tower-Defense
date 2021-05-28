using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class Temp : MonoBehaviour
    {
        public GameObject Tower;

        public int Cost => Tower.GetComponentInChildren<TowerBase>().Cost;
        public string Name => Tower.GetComponentInChildren<TowerBase>().Name;
    }
}
