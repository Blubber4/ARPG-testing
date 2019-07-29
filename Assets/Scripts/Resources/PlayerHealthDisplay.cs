using UnityEngine;
using UnityEngine.UI;

namespace RPG.Resources
{
    public class PlayerHealthDisplay : HealthDisplay
    {
        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
            healthText = GetComponent<Text>();
        }
    }
}
