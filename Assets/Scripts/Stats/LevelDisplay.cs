using UnityEngine.UI;
using UnityEngine;
using System;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        // cached references
        BaseStats baseStats;

        void Awake()
        {
            baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        protected virtual void Update()
        {
            GetComponent<Text>().text = String.Format("{0:0}", baseStats.GetLevel());
        }
    }
}
