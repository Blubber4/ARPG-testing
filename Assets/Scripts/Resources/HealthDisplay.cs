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

        // cached references
        protected Health health;
        protected Text healthText;

        [SerializeField] DisplayMode displayMode = DisplayMode.AsValue;
        //DisplayMode oldDisplayMode;  ***** commented out code is for when health bar is implemented, so player can have option of disabling/enabling text *****

        protected virtual void Update()
        {
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
            healthText.text = String.Format("Health: {0:0}%", health.GetPercentHealth());
        }
    }
}