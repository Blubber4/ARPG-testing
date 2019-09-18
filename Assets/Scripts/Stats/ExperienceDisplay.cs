using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    /* with some simple adjustment could reuse HealthDisplay code here to allow modes.
       I think I prefer this instead for experience, but maybe being able to hide it would be good? */
    public class ExperienceDisplay : MonoBehaviour 
    {
        // cached references
        Experience experience;

        void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        protected virtual void Update()
        {
            GetComponent<Text>().text = String.Format("{0:0}", experience.GetPoints());
        }
    }
}
