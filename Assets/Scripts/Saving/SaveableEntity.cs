using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = "";
        static Dictionary<string, SaveableEntity> globalLookup = new Dictionary<string, SaveableEntity>();

#if UNITY_EDITOR
        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;

            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");

            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            globalLookup[property.stringValue] = this;
        }
#endif

        private bool IsUnique(string candidateKey)
        {
            if (!globalLookup.ContainsKey(candidateKey)) return true;

            if (globalLookup[candidateKey] == this) return true;

            if (globalLookup[candidateKey] == null) // object has been destroyed, delete from global
            {
                globalLookup.Remove(candidateKey);
                return true;
            }

            if (globalLookup[candidateKey].GetUniqueIdentifier() != candidateKey)
            {
                globalLookup.Remove(candidateKey);
                return true;
            }

            return false;
        }

        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }

        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                state[saveable.GetType().ToString()] = saveable.CaptureState();
            }
            return state;
            // return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                string typeString = saveable.GetType().ToString();
                if (stateDict.ContainsKey(typeString))
                {
                    saveable.RestoreState(stateDict[typeString]);
                }
            }
            // SerializableVector3 position = (SerializableVector3)state;
            // GetComponent<NavMeshAgent>().enabled = false;
            // transform.position = position.ToVector();
            // GetComponent<NavMeshAgent>().enabled = true;
            // GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}