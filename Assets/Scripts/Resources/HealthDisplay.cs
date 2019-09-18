using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Resources
{
    public class HealthDisplay : MonoBehaviour
    {
        protected enum DisplayMode
        {
            AsValue,
            AsPercent,
            AsBarOnly,
        }

        // cached references - cached references should be assigned in Awake() of child class
        protected Health health;
        protected Text healthText;

        [SerializeField] DisplayMode displayMode = DisplayMode.AsValue;
        //DisplayMode oldDisplayMode;  ***** commented out code in this file is for when health bar is implemented, so player can have option of disabling/enabling text *****

        protected virtual void Update()
        {
            if (health == null || healthText == null)
            {
                Debug.LogError("Child class of HealthDisplay has not cached health or healthText properly. Try assigning in Awake().");
                return;
            }

            ShowHealthBar();
            /* 
            if (oldDisplayMode != displayMode)
            {
                DisplayModeChange();
            }
            */

            if (displayMode == DisplayMode.AsPercent)
            {
                ShowPercentHealth();
            }
            else if (displayMode == DisplayMode.AsValue)
            {
                ShowHealthValue();
            }
            else
            {
                ShowPercentHealth();
            }
            //oldDisplayMode = displayMode;
        }

        private void DisplayModeChange()
        {
            if (displayMode == DisplayMode.AsBarOnly)
            {
                GetComponent<Canvas>().enabled = false; // i don't think this method of disabling will work, probably need to change later
            }
            else
            {
                GetComponent<Canvas>().enabled = true;
            }
        }

        private void ShowHealthBar()
        {
            // TODO
        }

        private void ShowHealthValue()
        {
            healthText.text = health.GetHealth().ToString() + " / " + health.GetMaxhealth();
        }

        private void ShowPercentHealth()
        {
            healthText.text = String.Format("{0:0}%", health.GetPercentHealth());
        }
    }
}
