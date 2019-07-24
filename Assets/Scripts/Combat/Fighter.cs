using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        // config params
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float attackSpeed = .5f;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] GameObject weaponPrefab = null;
        [SerializeField] Transform handTransform = null;
        [SerializeField] AnimatorOverrideController weaponOverride = null;

        // cached references
        Mover mover;
        ActionScheduler actionScheduler;
        Animator animator;
        Health target;

        float timeSinceLastAttack = Mathf.Infinity;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            SpawnWeapon();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead()) return;

            if (!IsInRange())
            {
                mover.MoveTo(target.transform.position, 1f);
            }
            else
            {
                mover.Cancel();
                AttackBehavior();
            }
        }

        public void SpawnWeapon()
        {
            Instantiate(weaponPrefab, handTransform);
            Animator animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = weaponOverride;
        }

        private void AttackBehavior()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > attackSpeed)
            {
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
            animator.ResetTrigger("cancelAttack");
            // this will trigger the Hit() event
            animator.SetTrigger("attack");
        }

        // animation event, not called directly within code
        private void Hit()
        {
            if (target == null) return;
            target.TakeDamage(weaponDamage);
        }

        private bool IsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;

            Health targetToAttack = combatTarget.GetComponent<Health>();

            return targetToAttack != null && !targetToAttack.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            actionScheduler.StartAction(this); // cache action scheduler?
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
        }

        private void StopAttack()
        {
            animator.ResetTrigger("attack");
            animator.SetTrigger("cancelAttack");
        }
    }
}
