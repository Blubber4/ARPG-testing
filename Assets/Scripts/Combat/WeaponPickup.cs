using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        // config params
        [SerializeField] Weapon weapon = null;
        [SerializeField] float respawnTime = 5f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag != "Player") return;

            other.GetComponent<Fighter>().EquipWeapon(weapon);
            StartCoroutine(HideForSeconds(respawnTime));
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            SetPickupVisibility(false);
            yield return new WaitForSeconds(seconds);
            SetPickupVisibility(true);
        }

        private void SetPickupVisibility(bool isVisible)
        {
            gameObject.GetComponent<Collider>().enabled = isVisible;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(isVisible);
            }
        }
    }
}
