using UnityEngine;
using UnityEngine.UI;
using RPG.Resources;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : HealthDisplay
    {
        // cached references
        Fighter fighter;

        private void Awake()
        {
            healthText = GetComponent<Text>();
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        protected override void Update()
        {
            if (fighter.GetTarget() == null)
            {
                healthText.text = "N/A";
                return;
            }
            health = fighter.GetTarget();
            base.Update();
        }
    }
}
