using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        // config params
        [SerializeField] float healthPoints = 100f; // serialized for troubleshooting

        float maxHealth;
        bool isDead = false;

        private void Awake()
        {
            maxHealth = GetComponent<BaseStats>().GetStat(Stat.Health);
            healthPoints = maxHealth;
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if (healthPoints == 0)
            {
                Die();
                AwardExperienec(instigator);
            }
        }

        private void AwardExperienec(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        public float GetHealth()
        {
            return healthPoints;
        }

        public float GetPercentHealth()
        {
            return 100 * healthPoints / maxHealth;
        }

        public float GetMaxhealth()
        {
            return maxHealth;
        }

        // animation event
        private void Die()
        {
            if (isDead) return;

            isDead = true;
            GetComponent<Animator>().SetTrigger("die"); // cache animator?
            GetComponent<ActionScheduler>().CancelCurrentAction();
            CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
            if (capsuleCollider != null)
            {
                capsuleCollider.enabled = false;
            }
            Rigidbody rigidBody = GetComponent<Rigidbody>();
            if (rigidBody != null)
            {
                rigidBody.velocity = new Vector3(0, 0, 0); // to stop rare sliding glitches, will need to change later if add any kind of intentional ragdoll effects.
                rigidBody.isKinematic = false;
                rigidBody.detectCollisions = false;
            }
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;

            if (healthPoints <= 0)
            {
                Die();
            }
        }
    }
}
