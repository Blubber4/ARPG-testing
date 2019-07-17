using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        bool wasTriggered = false;
        private void OnTriggerEnter(Collider other)
        {
            if (!wasTriggered && other.gameObject.tag == "Player")
            {
                wasTriggered = true;
                GetComponent<PlayableDirector>().Play();
            }
        }
    }
}
