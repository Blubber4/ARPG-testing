using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        // config params
        [SerializeField] float healthPoints = 100f;

        bool isDead = false;

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if (healthPoints == 0)
            {
                Die();
            }
        }

        // animation event
        private void Die()
        {
            if (isDead) return;

            isDead = true;
            GetComponent<Animator>().SetTrigger("die"); // cache animator?
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}
