using UnityEngine;
using RPG.Core;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        // config params
        [SerializeField] float speed = 1f;
        [SerializeField] bool isHoming = true;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float maxLifeTime = 10f;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] GameObject[] destroyQuick = null;
        [SerializeField] float lifeAfterImpactQuick = 1f;
        [SerializeField] GameObject[] destroyExtended = null;
        [SerializeField] float lifeAfterImpactExtended = 5f;

        Health target = null;
        float damage = 0;

        private void Start()
        {
            if (target == null) return;

            transform.LookAt(GetAimLocation());
        }

        void Update()
        {
            if (target == null) return;

            if (isHoming && !target.IsDead())
            {
                transform.LookAt(GetAimLocation());
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, float damage)
        {
            this.target = target;
            this.damage = damage;

            Destroy(gameObject, maxLifeTime);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();

            if (targetCapsule == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * (targetCapsule.height / 1.7f); // divide by 1.7 for chest height
        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.GetComponent<Health>() != target) return;
            if (target.IsDead()) return;

            target.TakeDamage(damage);

            speed = 0;

            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }

            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }

            foreach (GameObject toDestroy in destroyQuick)
            {
                Destroy(toDestroy, lifeAfterImpactQuick);
            }
            foreach (GameObject toDestroy in destroyExtended)
            {
                ParticleSystem particleSystem = toDestroy.GetComponent<ParticleSystem>();
                if (particleSystem != null)
                {
                    var em = particleSystem.emission;
                    em.enabled = false;
                }
                Destroy(toDestroy, lifeAfterImpactExtended);
            }
            Destroy(gameObject, maxLifeTime);
        }
    }
}
