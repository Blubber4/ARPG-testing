using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        // config params
        [Range(1,30)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;

        public float GetStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, startingLevel);
        }

        public int GetLevel()
        {
            Experience experience = GetComponent<Experience>();
            if (experience == null) return startingLevel;

            float currentXP = GetComponent<Experience>().GetPoints();
            int maxLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass); // technically max level is one above this number.
            for (int levels = 1; levels < maxLevel; levels++)
            {
                float xpToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, levels);
                if (xpToLevelUp > currentXP)
                {
                    return levels;
                }
            }

            return maxLevel + 1;
        }
    }
}

