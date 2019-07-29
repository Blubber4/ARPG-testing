using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Resources;
using System;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        // cached references
        Health playerHealth;
        Fighter playerFighter;

        bool isAttackMove;
        bool isFirstClick; // for checking first click of an attack move, so we can avoid all clicks initiating a new attack move when one just needs to finish
        Vector3 attackMovePosition;

        private void Awake()
        {
            playerHealth = GetComponent<Health>();
            playerFighter = GetComponent<Fighter>();
        }

        private void Start()
        {
            isAttackMove = false;
            isFirstClick = true;
        }

        void Update()
        {
            if (playerHealth.IsDead()) return;
            if (InteractWithCombat()) return; // in order to prevent walking into enemies when already in range to attack, need to check if we've interacted with combat before moving
            if (AttackMove()) return; // attack move behavior should still attack the enemy if we click one directly
            if (InteractWithMovement()) return;
            print("Nothing to do");
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;

                Fighter fighter = GetComponent<Fighter>();

                if (!fighter.CanAttack(target.gameObject)) continue;

                if (Input.GetMouseButton(1) || (isAttackMove && Input.GetMouseButton(0)))
                {
                    isAttackMove = false;
                    fighter.Attack(target.gameObject);
                }
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(1))
                {
                    isAttackMove = false;
                    GetComponent<Mover>().StartMoveAction(hit.point, 1f);
                }
                return true;
            }
            return false;
        }

        private bool AttackMove()
        {
            if (Input.GetKey(KeyCode.A)) // pull this out into a variable if keybind customization is offered in future
            {
                // TODO: change cursor to reflect attack move, change back when attack move is done.
                isAttackMove = true;
                isFirstClick = true;
                print("Attack move activated");
                return true;
            }
            if (!isAttackMove) return false;

            if (Input.GetKey(KeyCode.Escape))
            {
                isAttackMove = false;
                isFirstClick = true;
                return true;
                // TODO: change cursor back to normal
            }

            if (isFirstClick && Input.GetMouseButton(0))
            {
                print(attackMovePosition);
                CreateAttackMove();
                return true;
            }

            if (!isFirstClick)
            {
                ContinueAttackMove();
                return false; // to ensure it allows breaking with movement/etc.
            }

            return false;
        }

        private void CreateAttackMove()
        {
            RaycastHit hit;
            bool onHit = Physics.Raycast(GetMouseRay(), out hit);

            Vector3 playerPosition = gameObject.transform.position;
            float weaponRange = playerFighter.GetEquippedWeapon().GetRange();

            // if hit is in attack range, check for enemy closest to that point and begin attacking them
            if (onHit && Vector3.Distance(playerPosition, hit.point) < weaponRange)
            {
                CombatTarget closestTarget = GetClosestTarget(hit.point, weaponRange);
                if (closestTarget != null)
                {
                    playerFighter.Attack(closestTarget.gameObject);
                    isAttackMove = false;
                    isFirstClick = true;
                    return;
                }
            }

            // no attackable enemy in range of hit, set attack move position
            if (onHit)
            {
                attackMovePosition = hit.point;
                isFirstClick = false;
                return;
            }
        }

        private void ContinueAttackMove()
        {
            // initial hit was not in attack range, or no enemy was in range of hit. Move until a target comes into attack range, then attack closest target
            float weaponRange = playerFighter.GetEquippedWeapon().GetRange();
            CombatTarget closestTarget = GetClosestTarget(gameObject.transform.position, weaponRange);

            if (closestTarget != null)
            {
                playerFighter.Attack(closestTarget.gameObject);
                return;
            }

            GetComponent<Mover>().StartMoveAction(attackMovePosition, 1f);
        }
           

        private CombatTarget GetClosestTarget(Vector3 origin, float range)
        {
            Collider[] hitColliders = Physics.OverlapSphere(origin, range);
            float closestColliderDistance = Mathf.Infinity;
            CombatTarget closestTarget = null;

            foreach (Collider collider in hitColliders)
            {
                CombatTarget currentTarget = collider.GetComponent<CombatTarget>();
                if (currentTarget == null) continue;

                float distance = Vector3.Distance(origin, collider.transform.position);
                if (distance < closestColliderDistance)
                {
                    closestColliderDistance = distance;
                    closestTarget = currentTarget;
                }
            }

            return closestTarget;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
